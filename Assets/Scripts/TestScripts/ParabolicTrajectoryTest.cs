using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//test script used to investigate different methods of creating curved/arched trajectories
public class ParabolicTrajectoryTest : MonoBehaviour
{
    public Vector3 startPos, endPos;
    public float arcHeight;
    [Range(0,1)]
    public float pathProgression;
    public bool useSineCurve;

    public float throwTime;

    private float time;
    private void Update()
    {
        CalculatePosition();

        //float yPos;
        //yPos = arcHeight * (transform.position.x - startPos.x) * (transform.position.x - endPos.x);
        //transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        time += Time.deltaTime;
        Vector3 curve = new Vector3(0f, CalculateCurve(time / throwTime), 0f);
        if (useSineCurve)
        {
            curve = new Vector3(0f, CalculateSineCurve(time / throwTime), 0f);
        }
        transform.position += curve * arcHeight;


        if (time > throwTime)
        {
            time = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(startPos, 0.1f);
        Gizmos.DrawSphere(endPos, 0.1f);
        Gizmos.DrawLine(startPos, endPos);
    }

    private void CalculatePosition() //before arc added
    {
        Vector3 distance = endPos - startPos;
        transform.position = startPos + (distance * time / throwTime);

        //arcHeight = distance.magnitude;
    }

    private float CalculateCurve(float pathProg)
    {
        float y = -(Mathf.Pow(pathProg, 2) - pathProg) * 4;
        return y; 
        // y= -(x^2 -x)    draws a curve like /\
        // curve starts and ends at x = 0, while y = 0.5, x = 1
    }

    private float CalculateSineCurve(float pathProg)
    {
        float y = Mathf.Sin(pathProg * Mathf.PI);
        return y;
    }
}
