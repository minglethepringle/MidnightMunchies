using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private int maxSlots = 10;
    private Transform container;
    private Transform inventoryItemTemplate;
    private Transform emptyInventoryItemTemplate;
    private static InventoryUIController instance;

    private static Dictionary<int, Item.ItemType> slotItems = new Dictionary<int, Item.ItemType>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        container = transform.Find("Container");
        inventoryItemTemplate = container.Find("ItemTemplate");
        emptyInventoryItemTemplate = container.Find("EmptyItemTemplate");
    }

    private void Update()
    {
        HandleHotkeyInput();
        UpdateInventoryUI();
    }

    private void HandleHotkeyInput()
    {
        for (int i = 1; i <= maxSlots && i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && slotItems.TryGetValue(i, out Item.ItemType itemType))
            {
                ActivateItem(itemType);
            }
        }
    }

    private void UpdateInventoryUI()
    {
        for (int i = 1; i <= maxSlots; i++)
        {
            if (slotItems.TryGetValue(i, out Item.ItemType itemType))
            {
                CreateOrUpdateItemSlot(i, itemType);
            }
            else
            {
                CreateOrUpdateEmptySlot(i);
            }
        }
    }

    private void CreateOrUpdateItemSlot(int slotIndex, Item.ItemType itemType)
    {
        Transform slotTransform = container.Find($"Slot_{slotIndex}");
        int siblingIndex = -1;
        if (slotTransform != null)
        {
            siblingIndex = slotTransform.GetSiblingIndex();
            Destroy(slotTransform.gameObject);
        }
        slotTransform = Instantiate(inventoryItemTemplate, container);
        slotTransform.name = $"Slot_{slotIndex}";
        if (siblingIndex != -1)
        {
            slotTransform.SetSiblingIndex(siblingIndex);
        }

        slotTransform.Find("ItemQuantity").GetComponent<Text>().text = 'x' + Inventory.GetItemCount(itemType).ToString();
        slotTransform.Find("ItemIcon").GetComponent<Image>().sprite = Item.GetIcon(itemType);

        RectTransform slotRectTransform = slotTransform.GetComponent<RectTransform>();
        slotRectTransform.anchoredPosition = new Vector2((slotIndex - 1) * 35f, 0);

        slotTransform.gameObject.SetActive(true);
    }

    private void CreateOrUpdateEmptySlot(int slotIndex)
    {
        Transform slotTransform = container.Find($"Slot_{slotIndex}");
        int siblingIndex = -1;
        if (slotTransform != null)
        {
            siblingIndex = slotTransform.GetSiblingIndex();
            Destroy(slotTransform.gameObject);
        }
        slotTransform = Instantiate(emptyInventoryItemTemplate, container);
        slotTransform.name = $"Slot_{slotIndex}";
        if (siblingIndex != -1)
        {
            slotTransform.SetSiblingIndex(siblingIndex);
        }

        RectTransform slotRectTransform = slotTransform.GetComponent<RectTransform>();
        slotRectTransform.anchoredPosition = new Vector2((slotIndex - 1) * 35f, 0);

        slotTransform.gameObject.SetActive(true);
    }

    public static void AddItemToSlot(Item.ItemType itemType)
    {
        // Check if the item already exists in any slot
        foreach (var slot in slotItems)
        {
            if (slot.Value == itemType)
            {
                // Item already exists, no need to add a new slot
                return;
            }
        }

        // If the item doesn't exist, find the first empty slot and add it
        for (int i = 1; i <= instance.maxSlots; i++)
        {
            if (!slotItems.ContainsKey(i))
            {
                slotItems[i] = itemType;
                return;
            }
        }
        Debug.LogWarning("No empty slots available to add item.");
    }

    public static void RemoveItemFromSlot(Item.ItemType itemType)
    {
        foreach (var slot in slotItems)
        {
            if (slot.Value == itemType)
            {
                slotItems.Remove(slot.Key);
                return;
            }
        }
    }

    private void ActivateItem(Item.ItemType itemType)
    {
        Debug.Log($"Activated item: {Item.GetName(itemType)}");

        switch (itemType)
        {
            case Item.ItemType.Pistol:
                PlayerWeaponManager.SwitchToPistol();
                break;
            case Item.ItemType.AssaultRifle:
                PlayerWeaponManager.SwitchToAssaultRifle();
                break;
            case Item.ItemType.Shotgun:
                PlayerWeaponManager.SwitchToShotgun();
                break;
            case Item.ItemType.RocketLauncher:
                PlayerWeaponManager.SwitchToRocketLauncher();
                break;
            case Item.ItemType.Grenades:
                PlayerWeaponManager.SwitchToGrenade();
                break;
            case Item.ItemType.Airstrikes:
                PlayerWeaponManager.SwitchToAirstrike();
                break;
            case Item.ItemType.Armor:
                PlayerPowerups.ActivateArmor();
                Inventory.RemoveItem(itemType);
                break;
            case Item.ItemType.SloMo:
                PlayerPowerups.ActivateSloMo();
                Inventory.RemoveItem(itemType);
                break;
            case Item.ItemType.MoDamage:
                PlayerPowerups.ActivateMoDamage();
                Inventory.RemoveItem(itemType);
                break;
            case Item.ItemType.MoAmmo:
                PlayerPowerups.ActivateMoAmmo();
                Inventory.RemoveItem(itemType);
                break;
            case Item.ItemType.MoBullets:
                PlayerPowerups.ActivateMoBullets();
                Inventory.RemoveItem(itemType);
                break;
        }
    }
}