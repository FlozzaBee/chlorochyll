using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScaleAgent))]
public class ScaleAgentEditor : Editor
{
    //External objects
    private ScaleAgent targetScript;

    //Foldouts & menu options
    private bool showInitialScaleFoldout = false;
    private bool showFinalScaleFoldout = false;
    private bool enableDebug = false;

    //Stored data
    private SerializedProperty copiedCurve;

    //serialized properties
    SerializedProperty objects;
    SerializedProperty startScales;
    SerializedProperty endScales;
    SerializedProperty scaleCurves;
    SerializedProperty currentScaleFactor;
    private void OnEnable()
    {
        objects = serializedObject.FindProperty("objects");
        startScales = serializedObject.FindProperty("startScales");
        endScales = serializedObject.FindProperty("endScales");
        scaleCurves = serializedObject.FindProperty("scaleCurves");
        currentScaleFactor = serializedObject.FindProperty("currentScaleFactor");
        // finds each of the variables we need to edit from the target script

        targetScript = (ScaleAgent)target;
        //targets the active scale agent script, to get information from it
    } 
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script should be applied to the root of of the growable object, not an empty above it", MessageType.Info);
        //base.OnInspectorGUI();

        if(GUILayout.Button("Get GameObject list"))
        {
            //targetScript.objects = new GameObject[targetScript.GetComponentsInChildren<Transform>().Length];
            objects.arraySize = targetScript.GetComponentsInChildren<Transform>().Length;
            int i = 0;
            foreach (Transform transform in targetScript.GetComponentsInChildren<Transform>())
            {
                //targetScript.objects[i] = transform.gameObject;
                objects.GetArrayElementAtIndex(i).objectReferenceValue = transform.gameObject;
                i++;
            }



        } // gets a list of all gameobjects under the object with the scale agent script in the hierarchy 


        EditorGUILayout.PropertyField(objects, new GUIContent("GameObject and child GameObjects"), true);
        // displays a list of all these gameobjects

        EditorGUILayout.Space(20f);

        if (GUILayout.Button("Store Initial Scales"))
        {
            //targetScript.startScales = new float[targetScript.objects.Length];
            //int i = 0;
            //foreach (GameObject obj in targetScript.objects)
            //{
            //    targetScript.startScales[i] = obj.transform.localScale.y;
            //    i++;
            //}
            startScales.arraySize = objects.arraySize;
            
            for (int i = 0; i < objects.arraySize; i++)
            {
                GameObject obj = objects.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
                startScales.GetArrayElementAtIndex(i).floatValue = obj.transform.localScale.y;
            }
        }

        showInitialScaleFoldout = EditorGUILayout.Foldout(showInitialScaleFoldout, "Initial Scales (" + startScales.arraySize + ")", true);
        if (showInitialScaleFoldout) // displays a foldout "initial scales (x) where x is number of scale values. Contains list of scales
        {
            EditorGUILayout.HelpBox("Each GameObjects Starting Scale", MessageType.None);

            for (int i = 0; i < startScales.arraySize; i++) 
            {
                string name = objects.GetArrayElementAtIndex(i).objectReferenceValue.name;
                EditorGUILayout.LabelField(new GUIContent(name), new GUIContent(startScales.GetArrayElementAtIndex(i).floatValue.ToString()));
            } 
        }

        EditorGUILayout.Space(20f);

        if(GUILayout.Button("Store Final Scales"))
        {
            //targetScript.endScales = new float[targetScript.objects.Length];
            //int i = 0;
            //foreach (GameObject obj in targetScript.objects)
            //{

            //    targetScript.endScales[i] = obj.transform.localScale.y;
            //    i++;
            //}

            endScales.arraySize = objects.arraySize;

            for (int i = 0; i < objects.arraySize; i++)
            {
                GameObject obj = objects.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
                endScales.GetArrayElementAtIndex(i).floatValue = obj.transform.localScale.y;
            }

        }
        showFinalScaleFoldout = EditorGUILayout.Foldout(showFinalScaleFoldout, "Final Scales (" + endScales.arraySize + ")", true);
        if (showFinalScaleFoldout)
        {
            EditorGUILayout.HelpBox("Each GameObjects final/fully grown scale", MessageType.None);
            for (int i = 0; i < endScales.arraySize; i++)
            {
                GameObject obj = objects.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
                EditorGUILayout.LabelField(new GUIContent(obj.name), new GUIContent(endScales.GetArrayElementAtIndex(i).floatValue.ToString()));
            }
        }

        EditorGUILayout.Space(20f);
        if(GUILayout.Button("GENERATE CURVES"))
        {
            //targetScript.scaleCurves = new AnimationCurve[targetScript.objects.Length];
            //int i = 0;
            //foreach(AnimationCurve curve in targetScript.scaleCurves)
            //{
            //    scaleCurves.arraySize = objects.arraySize;
            //    GenerateCurve(i);
            //    i++;
            //} // creates curves for start scale > end scale, for each game object in the model

            scaleCurves.arraySize = objects.arraySize;
            for (int i = 0; i < scaleCurves.arraySize; i++)
            {
                GenerateCurve(i);
            }

        }
        EditorGUILayout.HelpBox("Scale curves! (" + scaleCurves.arraySize + ")", MessageType.None);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleCurves"));
        if (targetScript.scaleCurves != null)
        {
            for (int i = 0; i < targetScript.scaleCurves.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty scaleCurve = (scaleCurves.GetArrayElementAtIndex(i)); // gets serialized property of just the current curve

                GameObject obj = objects.GetArrayElementAtIndex(i).objectReferenceValue as GameObject; 
                EditorGUILayout.PropertyField(scaleCurve, new GUIContent(obj.name)); //draws curve field with the corresponding gameobjects name as the lable
                                                                                                                                        //uses serialized properties to facilitate prefab saving. 

                if (GUILayout.Button("Copy", GUILayout.ExpandWidth(false)))
                {
                    Copy(i);
                } // copies curve
                if (GUILayout.Button("Paste", GUILayout.ExpandWidth(false)))
                {
                    Paste(i);
                } // pastes curve 
                GUILayout.Space(5);
                if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
                {
                    GenerateCurve(i);
                } // regenerates the curve back to the start scale > end scale curve
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.HelpBox("i am an empty void, no shred of my humanity remains, i am a husk", MessageType.None);

        enableDebug = EditorGUILayout.Toggle("Enable Preview Mode", enableDebug);
        if (enableDebug)
        {

            currentScaleFactor.floatValue = EditorGUILayout.Slider("Scale Progress" ,currentScaleFactor.floatValue, 0, 1);
            targetScript.Scale(0);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void Copy(int i)
    {
        copiedCurve = scaleCurves.GetArrayElementAtIndex(i);
        Debug.Log("Curve " + i + " copied");
    }
    private void Paste(int i)
    {
        scaleCurves.GetArrayElementAtIndex(i).animationCurveValue = copiedCurve.animationCurveValue;
        Debug.Log("Pasted to curve " + i);
    }

    private void GenerateCurve(int i)
    {
        scaleCurves.GetArrayElementAtIndex(i).animationCurveValue = new AnimationCurve(new Keyframe(0, startScales.GetArrayElementAtIndex(i).floatValue), new Keyframe(1, endScales.GetArrayElementAtIndex(i).floatValue));
    } // generates and sets a curve from the start scale and end scale values.
}


