using System;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject[] filledSlots = new GameObject[] { };

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
        for (int i = 0; i < orderedItems.Count && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateItem(orderedItems[i]);
            }
        }

        foreach (KeyValuePair<Item.ItemType, int> entry in Inventory.GetAllItems())
        {
            if (entry.Value > 0)
            {
                if (inventoryItems.ContainsKey(entry.Key))
                {
                    inventoryItems[entry.Key].transform.Find("ItemQuantity").GetComponent<Text>().text = 'x' + entry.Value.ToString();
                }
                else
                {
                    orderedItems.Add(entry.Key);
                    CreateInventoryItemUI(entry.Key, entry.Value, orderedItems.Count - 1);
                }
            }
            else
            {
                if (inventoryItems.ContainsKey(entry.Key))
                {
                    Destroy(inventoryItems[entry.Key]);
                    inventoryItems.Remove(entry.Key);
                }
            }
        }
    }

    private void CreateInventoryItemUI(Item.ItemType itemType, int count, int index)
    {
        Transform inventoryItemTransform = Instantiate(inventoryItemTemplate, container);
        RectTransform inventoryItemRectTransform = inventoryItemTransform.GetComponent<RectTransform>();

        float inventoryItemWidth = 35f;
        inventoryItemRectTransform.anchoredPosition = new Vector2(
            1 * index * inventoryItemWidth,
            0
        );

        Text itemName = inventoryItemTransform.Find("ItemName").GetComponent<Text>();
        Text itemQuantity = inventoryItemTransform.Find("ItemQuantity").GetComponent<Text>();
        // Text itemHotkey = inventoryItemTransform.Find("ItemHotkey").GetComponent<Text>();

        itemName.text = Item.GetName(itemType);
        itemQuantity.text = 'x' + count.ToString();
        // itemHotkey.text = (index + 1).ToString();

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
            1 * index * inventoryItemWidth,
            0
        );

        inventoryItemTransform.gameObject.SetActive(true);

        return inventoryItemTransform.gameObject;
    }

    private void ActivateItem(Item.ItemType itemType)
    {
        Debug.Log($"Activated item: {Item.GetName(itemType)}");

        if (itemType == activeItem)
        {
            PlayerWeaponManager.SwitchToPistol();
            activeItem = Item.ItemType.Subway8;
            return;
        }
        else if (itemType == Item.ItemType.AssaultRifle)
        {
            PlayerWeaponManager.SwitchToAssaultRifle();
        }
        else if (itemType == Item.ItemType.Shotgun)
        {
            PlayerWeaponManager.SwitchToShotgun();
        }
        else if (itemType == Item.ItemType.RocketLauncher)
        {
            PlayerWeaponManager.SwitchToRocketLauncher();
        }

        activeItem = itemType;
    }

    private void MakeInitialInventory()
    {
        Debug.Log("Making initial inventory");
        for (int index = 0; index < 10; index++)
        {
            Debug.Log($"Creating empty inventory item at index {index}");
            emptySlots[index] = CreateEmptyInventoryItemUI(index);
        }
    }
}