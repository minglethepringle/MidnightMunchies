using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private static HashSet<ItemType> purchasedItems = new HashSet<ItemType>();

    public enum ItemType
    {
        AssaultRifle,
        Shotgun,
        RocketLauncher,
        Armor,
        Grenades,
        Airstrikes,
        SloMo,
        MoAmmo,
        MoDamage,
        MoBullets
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.AssaultRifle: return 50;
            case ItemType.Shotgun: return 40;
            case ItemType.RocketLauncher: return 100;
            case ItemType.Armor: return 30;
            case ItemType.Grenades: return 20;
            case ItemType.Airstrikes: return 150;
            case ItemType.SloMo: return 70;
            case ItemType.MoAmmo: return 40;
            case ItemType.MoDamage: return 60;
            case ItemType.MoBullets: return 50;
            default: return 0;
        }
    }

    public static string GetName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.AssaultRifle: return "Assault Rifle";
            case ItemType.Shotgun: return "Shotgun";
            case ItemType.RocketLauncher: return "Rocket Launcher";
            case ItemType.Armor: return "Armor";
            case ItemType.Grenades: return "Grenades";
            case ItemType.Airstrikes: return "Airstrikes";
            case ItemType.SloMo: return "Slo-Mo";
            case ItemType.MoAmmo: return "Mo' Ammo";
            case ItemType.MoDamage: return "Mo' Damage";
            case ItemType.MoBullets: return "Mo' Bullets";
            default: return "Unknown Item"; // should never happen
        }
    }

    public static void PurchaseItem(ItemType itemType)
    {
        purchasedItems.Add(itemType);
    }

    public static bool IsItemPurchased(ItemType itemType)
    {
        return purchasedItems.Contains(itemType);
    }
}