using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//confines physics ui elements to their ui boxes
public class StayWithinBox : MonoBehaviour
{
    public GameObject targetGameObject;

    private Rigidbody rigidbody;
    

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        rigidbody.MovePosition(targetGameObject.transform.position);
    }

    
    
}
