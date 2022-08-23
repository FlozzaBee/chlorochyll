using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UIPhysics : MonoBehaviour
{
    public GameObject uiSeedContainer;
    public float forceMultiplier;

    new Rigidbody rigidbody;

    private Vector3 previousPosition;
    private void Start()
    {
        previousPosition = uiSeedContainer.transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //store global position 
        //difference btween currnt pos and last frame pos, 
        //apply difference as force to cube

        Vector3 force = uiSeedContainer.transform.position - previousPosition;
        rigidbody.AddForce(-force * forceMultiplier);
        previousPosition = uiSeedContainer.transform.position;
    }
}

