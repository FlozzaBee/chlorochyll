using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rough prototype - currently functional, but I want to make it MORE functional
public class ScaleAgent : MonoBehaviour
{
    //internal  variables
    public float currentScaleFactor = 0; // public to be edited by editor script

    //editor script controlled variables
    public GameObject[] objects; // array of all the game objects attached to this gameobject, including itself 
    public float[] startScales; // array of each gameobjects scales as it first appears
    public float[] endScales; // array of each gameobjects scales at maximum growth
    public AnimationCurve[] scaleCurves;


    private void Start()
    {
        Scale(0); // resets object to start scale on creation
    }

    public void Scale(float scaleAmount) // input a vewport point delta since last frame to grow plant with drag, called in PlayerControls
    {
        currentScaleFactor += scaleAmount;
        currentScaleFactor = Mathf.Clamp(currentScaleFactor, 0, 1);
        //Debug.Log("scale factor " + currentScaleFactor);
        for (int i = 0; i < objects.Length; i++)
        {
            float currentScale = scaleCurves[i].Evaluate(currentScaleFactor);
            objects[i].transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }
}
