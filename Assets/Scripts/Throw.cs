using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//throws a seed at a target location, called by PlayerControls tap
public class Throw : MonoBehaviour
{
    [Tooltip("Time for a seed to travel its thrown trajectory")]
    public float throwTime = 4;
    [Tooltip("how high the arc will be at its peak")]
    public float arcHeight;
    [Tooltip("what plant this seed turns into")]
    public GameObject plant;
    [Tooltip("how much to randomise the new plants scale by, 0 = no randomisation, 1 = 0%-200% scale ")]
    public float scaleRandomisation = 0.05f;

    private Vector3 dist;
    private Vector3 startPosition;
    private float time;
    private float lerpFraction; //fraction of how far through the throw we are, 0 to 1
    private Vector3 spinAxis;

    public void startThrow(Vector3 endPosition)
    {
        time = 0;
        startPosition = transform.position;
        dist = endPosition - startPosition; //stores some data on initial position and distance to that position 
        spinAxis = Random.onUnitSphere; //assigns a random spin axis for the seed to rotate about as its thrown
        StartCoroutine(ThrowUpdate(endPosition)); //starts throwUpdate routine, sending this coroutine the target location vector3
    }

    IEnumerator ThrowUpdate(Vector3 endPosition)
    {
        while(lerpFraction < 1) //loops until the throw is complete
        {
            lerpFraction = time / throwTime; //calculates a fraction (0 to 1) of progression through the desired length of throw (i.e. 2 seconds through a 4 second throw = 0.5)
            Vector3 moveTo = CalculatePosition(endPosition); //calculates the path as a straight line from start location to end location
            moveTo += CalculateCurve() * arcHeight; //adds an arc to that path, giving the look of a thrown object
            transform.position = moveTo; //sets position to calculated path

            SpinSeed(); //gives the seed a bit of spin :)
            time += Time.deltaTime; 
            yield return new WaitForFixedUpdate();
        }
        Plant(endPosition); //places the seeds corresponding plant at its end position
        Destroy(gameObject); //destroys this object when it finishes the throw 
    }
    private Vector3 CalculatePosition(Vector3 endPos) //before arc added
    {
        //Vector3 vector = startPosition + (dist * lerpFraction);
        Vector3 vector = Vector3.Lerp(startPosition, endPos, lerpFraction); //interpolates between start position and end position by lerpFraction
        return vector; 
    }
    private Vector3 CalculateCurve()
    {
        float y = -(Mathf.Pow(lerpFraction, 2) - lerpFraction) * 4;
        Vector3 vector = new Vector3(0, y, 0);
        return vector;
        // y= -(x^2 -x) * 4   draws a curve like /\
        // curve starts and ends at x = 0. while y = 0.5, x = 1.
    }

    private void Plant(Vector3 endPos)
    {
        GameObject newPlant = Instantiate(plant, endPos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0))); //instantiates plant with random Y rotation
        float randomfactor = (Random.Range(1 - scaleRandomisation, 1 + scaleRandomisation));
        Debug.Log("random factor " + randomfactor);
        newPlant.transform.localScale = newPlant.transform.localScale * randomfactor;
    }

    private void SpinSeed()
    {
        transform.rotation = Quaternion.AngleAxis(360 * lerpFraction, spinAxis);
    }
}
