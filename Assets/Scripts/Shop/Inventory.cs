using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Dictionary<Item.ItemType, int> inventoryItems = new Dictionary<Item.ItemType, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void AddItem(Item.ItemType itemType)
    {
        int amountToAdd = 1;

        if (itemType == Item.ItemType.Grenades)
        {
            amountToAdd = 1; // maybe configure later...
        }

        if (instance.inventoryItems.ContainsKey(itemType))
        {
            instance.inventoryItems[itemType] += amountToAdd;
        }
        else
        {
            instance.inventoryItems[itemType] = amountToAdd;
        }

        InventoryUIController.AddItemToSlot(itemType);
    }

    public static void RemoveItem(Item.ItemType itemType)
    {
        if (instance.inventoryItems.ContainsKey(itemType))
        {
            instance.inventoryItems[itemType]--;
            int itemCount = instance.inventoryItems[itemType];

            if (itemCount <= 0)
            {
                instance.inventoryItems.Remove(itemType);
                InventoryUIController.RemoveItemFromSlot(itemType);
                PlayerWeaponManager.SwitchToPistol();
            }
        }
    }

    public static bool HasItem(Item.ItemType itemType)
    {
        return instance.inventoryItems.ContainsKey(itemType) && instance.inventoryItems[itemType] > 0;
    }

    public static int GetItemCount(Item.ItemType itemType)
    {
        return instance.inventoryItems.ContainsKey(itemType) ? instance.inventoryItems[itemType] : 0;
    }
}