using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Health_1,
        Health_2,
        Health_3,
        Speed_1,
        Speed_2,
        Speed_3,
        Damage_1,
        Damage_2,
        Damage_3
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Health_1: return 50;
            case ItemType.Health_2: return 100;
            case ItemType.Health_3: return 150;
            case ItemType.Speed_1: return 50;
            case ItemType.Speed_2: return 100;
            case ItemType.Speed_3: return 150;
            case ItemType.Damage_1: return 50;
            case ItemType.Damage_2: return 100;
            case ItemType.Damage_3: return 150;
        }
    }
}
