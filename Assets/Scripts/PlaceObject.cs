using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    [SerializeField] private GameObject plantPrefab;

    [Tooltip("New objects will be spawned with a randomly picked scale between these values")]
    [SerializeField] private float minInitialScale;
    [SerializeField] private float maxInitialScale;

    public void Place(Vector3 location)
    {
        GameObject newPlant = Instantiate(plantPrefab, location, Quaternion.Euler(new Vector3(0, Random.Range(0,360), 0)));
        float initialScale = Random.Range(minInitialScale, maxInitialScale); // this is an overly complex way of randomising, changte later
        newPlant.transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }
}
