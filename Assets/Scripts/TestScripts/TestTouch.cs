using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    public InputManager inputManager;
    private Camera cameraMain;
    private bool isDragging= false;

    private void Awake()
    {
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        inputManager.OnEndTouch -= Move;
    }
    private void Move(Vector2 screenPosition, float time, float touchState)
    {
        /*Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoordinates = cameraMain.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates; */

        Ray ray = cameraMain.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.magenta, 1);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider != null)
            {
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }

    private void StartMove()
    {
        isDragging = true;
    }
    private void EndMove()
    {
        isDragging = false;
    }

    IEnumerator DragUpdate(GameObject clickedObject)
    {
        while (isDragging)
        {
            Debug.Log("touch state " + isDragging);
            yield return new WaitForFixedUpdate();
        }
    }

    private void Scale(Vector2 screenPosition, float time)
    {

    }
}
