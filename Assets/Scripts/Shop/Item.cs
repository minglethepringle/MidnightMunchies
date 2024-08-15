using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private static HashSet<ItemType> purchasedItems = new HashSet<ItemType>();

    public enum ItemType
    {
        Pistol,
        AssaultRifle,
        Shotgun,
        RocketLauncher,
        Armor,
        Grenades,
        Airstrikes,
        SloMo,
        MoAmmo,
        MoDamage,
        MoBullets,
        Subway1,
        Subway2,
        Subway3,
        Subway4,
        Subway5,
        Subway6,
        Subway7,
        Subway8
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Pistol: return 0;
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
            case ItemType.Subway1: return 10;
            case ItemType.Subway2: return 10;
            case ItemType.Subway3: return 10;
            case ItemType.Subway4: return 10;
            case ItemType.Subway5: return 10;
            case ItemType.Subway6: return 10;
            case ItemType.Subway7: return 5;
            default: return 0;
        }
    }

    public static string GetName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Pistol: return "Pistol";
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
            case ItemType.Subway1: return "Big Hot Pastrami";
            case ItemType.Subway2: return "Cold Cut Combo";
            case ItemType.Subway3: return "B.L.T.";
            case ItemType.Subway4: return "Black Forest Ham";
            case ItemType.Subway5: return "Meatball Marinara";
            case ItemType.Subway6: return "Spicy Italian";
            case ItemType.Subway7: return "Cookie";
            case ItemType.Subway8: return "Fountain Drink";
            default: return "Unknown Item";
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

    public static HashSet<ItemType> GetItemsPurchased()
    {
        return purchasedItems;
    }

    public static void SetItemsPurchased(HashSet<ItemType> items)
    {
        purchasedItems = items;
    }

    public static Sprite GetIcon(ItemType itemType)
    {
        string iconName = GetIconName(itemType);
        Sprite icon = Resources.Load<Sprite>("Icons/" + iconName);
        
        if (icon == null)
        {
            Debug.LogWarning($"Icon not found for {itemType}. Make sure the icon file exists in the Resources/Icons folder.");
            return null;
        }
        
        return icon;
    }

    private static string GetIconName(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Pistol: return "pistol_icon";
            case ItemType.AssaultRifle: return "assault_rifle_icon";
            case ItemType.Shotgun: return "shotgun_icon";
            case ItemType.RocketLauncher: return "rocket_launcher_icon";
            case ItemType.Armor: return "armor_icon";
            case ItemType.Grenades: return "grenades_icon";
            case ItemType.Airstrikes: return "airstrike_icon";
            case ItemType.SloMo: return "slo_mo_icon";
            case ItemType.MoAmmo: return "mo_ammo_icon";
            case ItemType.MoDamage: return "mo_damage_icon";
            case ItemType.MoBullets: return "mo_bullets_icon";
            case ItemType.Subway1: return "subway1_icon";
            case ItemType.Subway2: return "subway2_icon";
            case ItemType.Subway3: return "subway3_icon";
            case ItemType.Subway4: return "subway4_icon";
            case ItemType.Subway5: return "subway5_icon";
            case ItemType.Subway6: return "subway6_icon";
            case ItemType.Subway7: return "subway7_icon";
            case ItemType.Subway8: return "subway8_icon";
            default: return "unknown_icon";
        }
    }

    public static void DebugCheckIcons()
    {
        Debug.Log("Checking icons...");
        foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
        {
            string iconName = GetIconName(itemType);
            Sprite icon = Resources.Load<Sprite>("Icons/" + iconName);
            
            if (icon != null)
            {
                Debug.Log($"Icon for {itemType} loaded successfully. Path: Resources/Icons/{iconName}");
            }
            else
            {
                Debug.LogError($"Failed to load icon for {itemType}. Path: Resources/Icons/{iconName}");
            }
        }
    }
}