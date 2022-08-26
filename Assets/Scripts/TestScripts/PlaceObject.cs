using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is now obsolete, replaced with the Throw script
public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject plantPrefab;

    [Tooltip("How randomised the scale of placed objects is, 0 = the same scale every time")]
    [SerializeField] private float scaleRandomisation = 0;

    public void Place(Vector3 location)
    {
        GameObject newPlant = Instantiate(plantPrefab, location, Quaternion.Euler(new Vector3(0, Random.Range(0,360), 0)));
        float randFactor = plantPrefab.transform.localScale.y * (scaleRandomisation + 1) - plantPrefab.transform.localScale.y;
        float initialScale = Random.Range(plantPrefab.transform.localScale.y - randFactor, plantPrefab.transform.localScale.y + randFactor); // this is an overly complex way of randomising, changte later // i changed itbut itss still over compplicated and its 130 am fuck im going to wales tmr ornging
        newPlant.transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }
}
