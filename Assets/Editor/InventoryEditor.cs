using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryPersistant))]
public class InventoryEditor : Editor
{
    private InventoryPersistant invPersistant;
    private bool invItemsFoldout = false;

    //Serialized properties (properties from the target script)
    SerializedProperty heldItemIndexes;

    //Add item variables
    string[] options;
    int index = 0; //index of the item to be added

    private void OnEnable()
    {
        invPersistant = (InventoryPersistant)target;
        options = ItemDatabase.instance.itemCodes.ToArray();
        heldItemIndexes = serializedObject.FindProperty("heldItemIndexes");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //Fold out that shows each item held in the inventory! 
        EditorGUILayout.PropertyField(heldItemIndexes);


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Item to add", MessageType.None);
        index = EditorGUILayout.Popup(index, options);
        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("Add Item"))
        {
            invPersistant.AddItem(ItemDatabase.instance.itemDatabase[index].itemCode); //uses the target scripts addItem method to add the desired item, by getting its item code from the database
            //this is all kinda stupid and badly designed :/
        }

        serializedObject.ApplyModifiedProperties();
    }
}
