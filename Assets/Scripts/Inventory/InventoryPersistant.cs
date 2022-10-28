using System.Collections.Generic;
using UnityEngine;

public class InventoryPersistant : MonoBehaviour
{
    //public ItemDatabase database;

    
    public static InventoryPersistant Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<int> heldItemIndexes; //the items the player has in their inventory, as defined by their strings

    public void AddItem(string itemCode)
    {
        Debug.Log("adding " + itemCode);
        heldItemIndexes.Add(ItemDatabase.instance.itemCodes.IndexOf(itemCode));

        for (int i = 0; i < heldItemIndexes.Count; i++)
        {
            Debug.Log("Inventory slot " + i + " contains: " + ItemDatabase.instance.itemDatabase[heldItemIndexes[i]].itemDisplayName); //prints "inventory slot x contains: [item display name] for each item in the inventory
        }
    }
}
