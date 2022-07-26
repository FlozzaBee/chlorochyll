using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerControls : MonoBehaviour
{

    //inputs
        //primary touch
    private InputAction touchPress; 
    private InputAction touchPosition;
    private InputAction touchTap;
        //secondary touch
    private InputAction touchPress_1;
    private InputAction touchPosition_1;

    //external objects
    private PlayerInput playerInput;
    private Camera mainCamera;
    private PlaceObject placeObject;
    GameObject targetedObject; //used to store which object has been tapped on
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera; //must be assigned in editor to control zoom

    //internal variables
    private bool isTouching = false;
    int touchCount;
    Vector2 initialViewportPoint_1;
    private CinemachineFramingTransposer transposer;

    //smooth damp ref variables
    private float refSpin;
    private float refScale;

    //editor variables
    [Tooltip("Empty the camera is child of, should be in the center of the scene")]
    public GameObject cameraController;
    [Tooltip("distance moved from from initial tap to register a swipe, 0 to 1")]
    public float swipeThreshold = 0.01f;
    [Tooltip("How far a swipe rotates around the pivot")]
    public float spinAmountMultiplier = 200f;
    [Tooltip("Speed of the rotation around the pivot")]
    public float spinSmoothSpeed = 0.2f;

    [Header("Grow control parameters")]
    [Tooltip("how far you have to swipe to fully grow a plant, high number = less distance swiped, 1 = entire screen")]
    [Range(0.5f,3f)] public float growSpeed = 1f;
    //public float scaleAmountMultiplier = 0.5f;
    //public float scaleSmoothSpeed = 0.2f;

    [Tooltip("how far the camera can move in and out of the scene")]
    [Header("Zoom Parameters")]
    public float zoomMin = 1f;
    public float zoomMax = 30f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPress = playerInput.actions["TouchPress"];
        touchPosition = playerInput.actions["TouchPosition"];
        touchPress_1 = playerInput.actions["TouchPress_1"];
        touchPosition_1 = playerInput.actions["TouchPosition_1"];
        touchTap = playerInput.actions["TouchTap"];
        mainCamera = Camera.main;
        placeObject = gameObject.GetComponent<PlaceObject>(); // gets the placeObject script assigned to this gameobject if one is present
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>(); //gets framing transposer component for use in zoom
    }

    private void OnEnable()
    {
        touchPress.performed += StartTouch;
        touchPress.canceled += EndTouch;
        touchPress_1.performed += StartSecondaryTouch;
        touchTap.performed += Tap;
    } 

    private void OnDisable()
    {
        touchPress.performed -= StartTouch;
        touchPress.canceled -= EndTouch;
        touchPress_1.performed -= StartSecondaryTouch;
        touchTap.performed -= Tap;
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log(tap.ReadValue<float>());
        isTouching = true;
        StartCoroutine(SwipeUpdate());
        touchCount = 1;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        isTouching = false;
        touchCount = 0;
    }



    IEnumerator SwipeUpdate()
    {
        bool inputIdentified = false; 
        char inputType = '0'; //x for horizontal, y for vertical, p for pinch, 0 for unassigned/unidentified
        Vector2 initialViewportPoint = mainCamera.ScreenToViewportPoint(touchPosition.ReadValue<Vector2>()); //gets the initial touch point
        Vector3 initialCameraController = cameraController.transform.eulerAngles; //gets the camera controllers initial rotation
        Vector3 initialScale = Vector3.zero; //defines initial scale to be used later
        ScaleAgent scaleAgent = null; // defines the scale agent script of the targeted object 

        targetedObject = null; // resets the targeted object from previous tap
        Ray ray = mainCamera.ScreenPointToRay(touchPosition.ReadValue<Vector2>()); //casts ray to point touched 
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.magenta, 1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Growable")
            {
                targetedObject = hit.collider.gameObject;
                //initialScale = hit.collider.gameObject.transform.localScale; //if ray hits a growable object, store info about it
                scaleAgent = targetedObject.GetComponent<ScaleAgent>(); //assigns scale agent to that of the target object
            }
        }

        //for use in zoom
        float previousDistance = 0f, currentDistance = 0f;

        //for use in scale / vertical swipe
        Vector2 previousViewportPoint = Vector2.zero;

        while (isTouching) //repeats like an update while the screen is pressed 
        {
            Vector2 currentViewportPoint = mainCamera.ScreenToViewportPoint(touchPosition.ReadValue<Vector2>());
            Vector2 deltaViewportPoint = currentViewportPoint - initialViewportPoint; //calculates difference between initial touch position and current touch position, can be between 0 and 1
            
            if (!inputIdentified)
            {
                if (touchCount == 1)
                {
                    //Debug.Log("swipe delta " + deltaScreenSpace);
                    if (Mathf.Abs(deltaViewportPoint.x) > swipeThreshold)
                    {
                        inputType = 'x';
                        inputIdentified = true;
                    }
                    if (Mathf.Abs(deltaViewportPoint.y) > swipeThreshold)
                    {
                        inputType = 'y';
                        inputIdentified = true;
                        previousViewportPoint = mainCamera.ScreenToViewportPoint(touchPosition.ReadValue<Vector2>());
                    }
                }
                if (touchCount == 2)
                {
                    inputType = 'p';
                    inputIdentified = true;
                    previousDistance = Vector2.Distance(mainCamera.ScreenToViewportPoint(touchPosition.ReadValue<Vector2>()), mainCamera.ScreenToViewportPoint(touchPosition_1.ReadValue<Vector2>()));
                }
            }//finds the swipe direction by comparing it to a threshold
            else
            {
                if (inputType == 'x')
                {
                    Orbit(initialCameraController, deltaViewportPoint);
                } //if horizontal swipe direction was identified, spin camera controller by swipe amount

                if (inputType =='y')
                {
                    Debug.Log("swipe direction y");
                    if (targetedObject != null)
                    {

                        float deltaY = currentViewportPoint.y - previousViewportPoint.y;
                        scaleAgent.Scale(deltaY * growSpeed);
                        previousViewportPoint = currentViewportPoint;

                        //Grow(initialScale, deltaViewportPoint);
                    }
                } // if verticle swipe was identified, and a growable object was targeted, scale that object by swipe amount

                if(inputType == 'p')
                {
                    //pinch to zoom 
                    currentDistance = Vector2.Distance(mainCamera.ScreenToViewportPoint(touchPosition.ReadValue<Vector2>()), mainCamera.ScreenToViewportPoint(touchPosition_1.ReadValue<Vector2>()));

                    float targetPosition = transposer.m_CameraDistance;
                    targetPosition -= (currentDistance - previousDistance) * 40f;
                    Debug.Log("targetPosition " + targetPosition); 
                    
                    transposer.m_CameraDistance = Mathf.Clamp(targetPosition, zoomMin, zoomMax); ;
                    previousDistance = currentDistance; // cinemachine my beloved, you saved me //gets change in pinchedness between current and previous frame, adds that to the camera distance
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void Orbit(Vector3 initialCameraController, Vector2 deltaViewportPoint)
    {
        float smoothedRotation = Mathf.SmoothDampAngle(cameraController.transform.eulerAngles.y, initialCameraController.y + (deltaViewportPoint.x * spinAmountMultiplier), ref refSpin, spinSmoothSpeed);
        cameraController.transform.eulerAngles = new Vector3(
            initialCameraController.x,
            smoothedRotation,
            initialCameraController.z); //spinnnn 
    }

    private void StartSecondaryTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Secondary Touch Triggered " + touchPress_1.ReadValue<float>());
        Debug.Log("secondary touch position " + touchPosition_1.ReadValue<Vector2>());
        touchCount = 2;
        initialViewportPoint_1 = mainCamera.ScreenToViewportPoint(touchPosition_1.ReadValue<Vector2>());
    }

    private void Tap(InputAction.CallbackContext context)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition.ReadValue<Vector2>());
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan, 1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "PlantableSurface" && placeObject != null) 
            {
                placeObject.Place(hit.point);
            } // if player taps on a plantable surface, and the place object script is present on this gameobject, place an object
        }
    }

}
