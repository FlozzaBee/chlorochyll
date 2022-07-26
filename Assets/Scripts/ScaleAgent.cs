using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAgent : MonoBehaviour
{
    public AnimationCurve curve;
    [Range(0,1)]
    public float curvePoint;

    private void Update()
    {
        float newScale = curve.Evaluate(curvePoint);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
