using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryPersistant))]
public class InventoryEditor : Editor
{
    private InventoryPersistant invPersistant;
    private void OnEnable()
    {
        invPersistant = (InventoryPersistant)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Add Item"))
        {
            invPersistant.AddItem("seed:willow");
        }
    }
}
