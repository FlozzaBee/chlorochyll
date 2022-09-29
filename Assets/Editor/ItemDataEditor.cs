using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//Customises the layout and functionallity of the InventoryScriptableObject 
[CustomEditor(typeof(ItemDatabase))]
public class ItemDataEditor : Editor //This is confusingly named and should be changed
{
    SerializedProperty itemDatabase;
    
    SerializedProperty itemCodes; //List of all the item codes, to make accessing the item database struct list easier.

    private void OnEnable()
    {
        //Assign each serialized property to the corresponding variable from the scriptable object.
        itemDatabase = serializedObject.FindProperty("itemDatabase");
        itemCodes = serializedObject.FindProperty("itemCodes");
    }

    public override void OnInspectorGUI()
    {
        
        EditorGUILayout.HelpBox("This is a database of all the items in the game", MessageType.Info);

        //Draw lables for each field
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.HelpBox("Item Code", MessageType.None);
        EditorGUILayout.HelpBox("Item Display Name", MessageType.None);
        EditorGUILayout.HelpBox("Item Prefab", MessageType.None);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(itemDatabase);

        GUI.enabled = false;
        EditorGUILayout.PropertyField(itemCodes); //Dont edit this, its just to check that the list of codes is being created
        GUI.enabled = true;
        //DrawDefaultInspector();

        //for (int i = 0; i < itemCodes.arraySize; i++)
        //{
        //    EditorGUILayout.BeginHorizontal();

        //    //Get the property within each array of properties
        //    SerializedProperty itemCode = itemCodes.GetArrayElementAtIndex(i);
        //    SerializedProperty itemDisplayName = itemDisplayNames.GetArrayElementAtIndex(i);
        //    SerializedProperty itemPrefab = itemPrefabs.GetArrayElementAtIndex(i);
        //    //Draw those properties in a horizontal row
        //    EditorGUILayout.PropertyField(itemCode, GUIContent.none);
        //    EditorGUILayout.PropertyField(itemDisplayName, GUIContent.none);
        //    EditorGUILayout.PropertyField(itemPrefab, GUIContent.none);

        //    EditorGUILayout.EndHorizontal();
        //}

        //if (GUILayout.Button("Create new item entry"))
        //{
        //    itemCodes.arraySize++;
        //    itemDisplayNames.arraySize++;
        //    itemPrefabs.arraySize++;
        //}

        itemCodes.arraySize = itemDatabase.arraySize;
        for (int i = 0; i < itemDatabase.arraySize; i++)
        {
            SerializedProperty code = itemDatabase.GetArrayElementAtIndex(i).FindPropertyRelative("itemCode"); // < - access the variable "itemCode" within the specified ItemData struct from the ItemDatabase!!! 
            itemCodes.GetArrayElementAtIndex(i).stringValue = code.stringValue;
            //itemCodes.GetArrayElementAtIndex(i).stringValue = itemDatabase.GetArrayElementAtIndex(i).
        }

        serializedObject.ApplyModifiedProperties();
    }
}
