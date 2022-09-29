using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Custom property drawer for the item data struct, allowing item data properties to display in a row in the ItemDatabase scriptable object.
[CustomPropertyDrawer(typeof(ItemData))]
public class ItemDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //Get the properties we are looking to modify and asign them to serialized properties
        SerializedProperty itemCode = property.FindPropertyRelative("itemCode");
        SerializedProperty itemDisplayName = property.FindPropertyRelative("itemDisplayName");
        SerializedProperty itemPrefab = property.FindPropertyRelative("itemPrefab");

        //Calculate required rects to draw properties
        float widthSize = position.width / 3; //Increase this denominator and add a pos to add new properties
        float offsetSize = 2; //The dist between different property fields

        Rect pos1 = new Rect(position.x, position.y, widthSize - offsetSize, position.height);
        Rect pos2 = new Rect(position.x + widthSize * 1, position.y, widthSize - offsetSize, position.height);
        Rect pos3 = new Rect(position.x + widthSize * 2, position.y, widthSize, position.height);

        

        //In a horizontal layout, display those serialized properties.
        EditorGUI.PropertyField(pos1, itemCode, GUIContent.none);
        EditorGUI.PropertyField(pos2, itemDisplayName, GUIContent.none);
        EditorGUI.PropertyField(pos3, itemPrefab, GUIContent.none);

        GUIStyle labelStyle = GUIStyle.none;
        labelStyle.normal.textColor = Color.gray;
        //If the properties have not been set, create a label over their position
        if (itemCode.stringValue == "")
        {
            EditorGUI.LabelField(pos1, "  Item code", labelStyle);
        }
        if(itemDisplayName.stringValue == "")
        {
            EditorGUI.LabelField(pos2, "  Item display name", labelStyle);
        }
        //if(itemPrefab.objectReferenceValue == null)
        //{
        //    EditorGUI.LabelField(pos3, "  Item prefab", labelStyle);
        //}

        EditorGUI.EndProperty();

        //base.OnGUI(position, property, label);
    }
}
