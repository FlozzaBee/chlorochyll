using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rough prototype - currently functional, but I want to make it MORE functional
public class ScaleAgent : MonoBehaviour
{
    
    

    //internal  variables
    [Range(0,1)]
    private float currentScaleFactor = 0;
    private Vector3[] initialScales;
    private Transform[] objectHierarchy; // all of the gameObjects under the object this script is attached to, including this object

    //editor variables
    [Tooltip("each part of the model needs a curve. defines how each part scales. i.e. if a scalecurve starts at 0.5, the corresponding object will only start scaling when the growable object is half way through growth")]
    public AnimationCurve[] scaleCurve;
    [Tooltip("multiplies how large a model can get, useful for small models that you want to grow B I G, or big models you want to keep small ")]
    public float scaleMultiplier = 1f;

    private void Start()
    {
        objectHierarchy = GetComponentsInChildren<Transform>();
        initialScales = new Vector3[objectHierarchy.Length]; // creates array of initial scales the same length as the number of objects in the hierarchy
        int i = 0;
        foreach (Transform trans in objectHierarchy)
        {
            initialScales[i] = trans.localScale;
            i++;
        }
        
    }

    public void Scale(float scaleAmount)
    {
        currentScaleFactor += scaleAmount;
        currentScaleFactor = Mathf.Clamp(currentScaleFactor, 0, 1);
        Debug.Log("scale factor " + currentScaleFactor);
        /*
        int i = 0;
        foreach (Transform objTransform in objectHierarchy)
        {
            float currentScale = scaleCurve[i].Evaluate(currentScaleFactor) * scaleMultiplier;
            objTransform.localScale = initialScale + new Vector3(currentScale, currentScale, currentScale); 
            i++;
        } // scales each sub part of a model by its own unique curve :)
        */
        for (int i = 0; i < objectHierarchy.Length; i++)
        {
            float currentScale = scaleCurve[i].Evaluate(currentScaleFactor) * scaleMultiplier;
            Debug.Log("curent scale curve" + scaleCurve[i].Evaluate(0.5f));
            objectHierarchy[i].localScale = initialScales[i] + new Vector3(currentScale, currentScale, currentScale);
        }
    }
}
