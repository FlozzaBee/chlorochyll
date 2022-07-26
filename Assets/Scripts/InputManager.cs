using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{

    public delegate void StartTouchEvent(Vector2 position, float time, float touchState);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time, float touchState);
    public event EndTouchEvent OnEndTouch;

    private TouchControls touchControls;

    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("touch started " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        Debug.Log("Input Manager - readValue<float> " + context.ReadValue<float>());
        if (OnStartTouch != null)
        {
            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime, context.ReadValue<float>());
        }
    }
    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("touch ended" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        Debug.Log("Input Manager - readValue<float> " + context.ReadValue<float>());
        if (OnEndTouch != null)
        {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time, context.ReadValue<float>());
        }
    }
}
