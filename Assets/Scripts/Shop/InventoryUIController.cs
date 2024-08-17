using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    private Transform container;
    private Transform inventoryItemTemplate;
    private Transform emptyInventoryItemTemplate;
    private static InventoryUIController instance;

    private Dictionary<Item.ItemType, GameObject> inventoryItems = new Dictionary<Item.ItemType, GameObject>();
    private List<Item.ItemType> orderedItems = new List<Item.ItemType>();
    private GameObject[] emptySlots = new GameObject[10];
    private GameObject[] filledSlots = new GameObject[10];

    private Item.ItemType activeItem;

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

    private void Start()
    {
        MakeInitialInventory();
    }

    private void Update()
    {
        HandleHotkeyInput();
        UpdateInventoryUI();
    }

    private void HandleHotkeyInput()
    {
        for (int i = 0; i < orderedItems.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateItem(orderedItems[i]);
            }
        }
    }

    private void UpdateInventoryUI()
    {
        foreach (Item.ItemType itemType in Enum.GetValues(typeof(Item.ItemType)))
        {
            if (!Inventory.HasItem(itemType))
            {
                RemoveItem(itemType);
            }
            else
            {
                AddOrUpdateItem(itemType, Inventory.GetItemCount(itemType));
            }
        }
    }

    private void AddOrUpdateItem(Item.ItemType itemType, int count)
    {
        if (inventoryItems.ContainsKey(itemType))
        {
            UpdateItemQuantity(itemType, count);
        }
        else
        {
            AddNewItem(itemType, count);
        }
    }

    private void UpdateItemQuantity(Item.ItemType itemType, int count)
    {
        if (inventoryItems.TryGetValue(itemType, out GameObject itemObject))
        {
            itemObject.transform.Find("ItemQuantity").GetComponent<Text>().text = 'x' + count.ToString();
        }
        else
        {
            Debug.LogWarning($"Tried to update quantity for {itemType}, but it wasn't in the inventoryItems dictionary.");
        }
    }

    private void AddNewItem(Item.ItemType itemType, int count)
    {
        int index = FindNextEmptySlot();
        if (index != -1)
        {
            orderedItems.Add(itemType);
            CreateInventoryItemUI(itemType, count, index);
        }
        else
        {
            Debug.LogWarning("No empty slots available to add new item.");
        }
    }

    private int FindNextEmptySlot()
    {
        for (int i = 0; i < emptySlots.Length; i++)
        {
            if (emptySlots[i] != null && emptySlots[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }

    private void RemoveItem(Item.ItemType itemType)
    {
        int count = Inventory.GetItemCount(itemType);
        if (count > 1)
        {
            UpdateItemQuantity(itemType, count - 1);
        }
        else
        {
            int index = orderedItems.IndexOf(itemType);
            
            if (index != -1)
            {
                if (filledSlots[index] != null)
                {
                    Destroy(filledSlots[index]);
                    filledSlots[index] = null;
                }
                emptySlots[index].SetActive(true);
                inventoryItems.Remove(itemType);
                orderedItems.RemoveAt(index);

                // Reposition remaining items
                RepositionItems();

                PlayerWeaponManager.SwitchToPistol();
            }
        }
    }

    private void RepositionItems()
    {
        for (int i = 0; i < orderedItems.Count; i++)
        {
            if (filledSlots[i] != null)
            {
                RectTransform itemRectTransform = filledSlots[i].GetComponent<RectTransform>();
                float inventoryItemWidth = 35f;
                itemRectTransform.anchoredPosition = new Vector2(i * inventoryItemWidth, 0);
            }
        }

        // Move empty slots to the end
        for (int i = orderedItems.Count; i < emptySlots.Length; i++)
        {
            if (emptySlots[i] != null)
            {
                RectTransform emptySlotRectTransform = emptySlots[i].GetComponent<RectTransform>();
                float inventoryItemWidth = 35f;
                emptySlotRectTransform.anchoredPosition = new Vector2(i * inventoryItemWidth, 0);
                emptySlots[i].SetActive(true);
            }
        }
    }

    private void CreateInventoryItemUI(Item.ItemType itemType, int count, int index)
    {
        Transform inventoryItemTransform = Instantiate(inventoryItemTemplate, container);
        inventoryItemTransform.SetSiblingIndex(container.childCount - index - 1);

        Text itemQuantity = inventoryItemTransform.Find("ItemQuantity").GetComponent<Text>();
        itemQuantity.text = 'x' + count.ToString();
        Image iconImage = inventoryItemTransform.Find("ItemIcon").GetComponent<Image>();
        iconImage.sprite = Item.GetIcon(itemType);

        inventoryItemTransform.gameObject.SetActive(true);

        inventoryItems[itemType] = inventoryItemTransform.gameObject;

        filledSlots[index] = inventoryItemTransform.gameObject;
        emptySlots[index].SetActive(false);
    }

    private GameObject CreateEmptyInventoryItemUI(int index)
    {
        Transform inventoryItemTransform = Instantiate(emptyInventoryItemTemplate, container);
        RectTransform inventoryItemRectTransform = inventoryItemTransform.GetComponent<RectTransform>();

        float inventoryItemWidth = 35f;
        inventoryItemRectTransform.anchoredPosition = new Vector2(
            index * inventoryItemWidth,
            0
        );

        inventoryItemTransform.gameObject.SetActive(true);

        return inventoryItemTransform.gameObject;
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
                RemoveItem(itemType);
                break;
            case Item.ItemType.SloMo:
                PlayerPowerups.ActivateSloMo();
                Inventory.RemoveItem(itemType);
                RemoveItem(itemType);
                break;
            case Item.ItemType.MoDamage:
                PlayerPowerups.ActivateMoDamage();
                Inventory.RemoveItem(itemType);
                RemoveItem(itemType);
                break;
            case Item.ItemType.MoAmmo:
                PlayerPowerups.ActivateMoAmmo();
                Inventory.RemoveItem(itemType);
                RemoveItem(itemType);
                break;
            case Item.ItemType.MoBullets:
                PlayerPowerups.ActivateMoBullets();
                Inventory.RemoveItem(itemType);
                RemoveItem(itemType);
                break;
        }

        activeItem = itemType;
    }

    private void MakeInitialInventory()
    {
        for (int index = 0; index < 10; index++)
        {
            emptySlots[index] = CreateEmptyInventoryItemUI(index);
        }
    }
}