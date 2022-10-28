using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//Allows creation of an Item Data file which stores information about each item in the game
[CreateAssetMenu(fileName = "Item Database", menuName = "Scriptable objects/Item Database")]
public class ItemDatabase : ScriptableSingleton<ItemDatabase>
{
    public List<ItemData> itemDatabase; //The list of structs that forms the item database
    public List<string> itemCodes; //A list of item codes that is used to more easily access info from the list of structs
    
}
