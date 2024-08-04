using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    private Transform container;
    private Transform inventoryItemTemplate;

    private static PlayerLookController playerLookController;
    private static InventoryUIController instance;

    private Text alert;

    private Dictionary<Item.ItemType, GameObject> inventoryItems = new Dictionary<Item.ItemType, GameObject>();

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
        
        Transform alertTransform = transform.Find("Alert");
        if (alertTransform != null)
        {
            alert = alertTransform.GetComponent<Text>();
            if (alert == null)
            {
                Debug.LogWarning("Alert Text component not found on Alert object.");
            }
        }
        else
        {
            Debug.LogWarning("Alert object not found in InventoryUIController.");
        }
    }

    private void Start()
    {
        if (playerLookController == null)
        {
            playerLookController = Camera.main.GetComponent<PlayerLookController>();
        }

        UpdateInventoryUI();

        Hide();
    }

    public void UpdateInventoryUI()
    {
        inventoryItemTemplate.gameObject.SetActive(false);
        
        foreach (var item in inventoryItems.Values)
        {
            Destroy(item);
        }
        inventoryItems.Clear();
        foreach (Transform child in container)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        Dictionary<Item.ItemType, int> allItems = Inventory.GetAllItems();

        int index = 0;
        foreach (var kvp in allItems)
        {
            CreateInventoryItemUI(kvp.Key, kvp.Value);
            index++;
        }
    }

    private void CreateInventoryItemUI(Item.ItemType itemType, int count)
    {
        Transform inventoryItemTransform = Instantiate(inventoryItemTemplate, container);
        RectTransform inventoryItemRectTransform = inventoryItemTransform.GetComponent<RectTransform>();

        float inventoryItemHeight = 30f;
        inventoryItemRectTransform.anchoredPosition = new Vector2(
            -50,
            60 + (-inventoryItemHeight * container.childCount)
        );

        Text itemName = inventoryItemTransform.Find("ItemName").GetComponent<Text>();
        Text itemPrice = inventoryItemTransform.Find("ItemQuantity").GetComponent<Text>();

        itemName.text = Item.GetName(itemType);
        itemPrice.text = 'x' + count.ToString();

        inventoryItemTransform.gameObject.SetActive(true);

        // Button button = inventoryItemTransform.GetComponent<Button>();
        // button.onClick.AddListener(() =>
        // {
        //     TryToBuyItem(itemType);
        // });

        // inventoryItems[itemType] = inventoryItemTransform.gameObject;

        // UpdateItemUI(itemType);
    }

    private void SetAlertText(string text)
    {
        if (alert == null)
        {
            Debug.LogWarning("Cannot set alert text: Alert Text component is null.");
            return;
        }

        alert.text = text;
        StartCoroutine(RemoveAlertText());
    }

    private IEnumerator RemoveAlertText()
    {
        if (alert == null) yield break;

        yield return new WaitForSeconds(2);
        alert.text = "";
    }

    public static void Hide()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(false);
            if (playerLookController != null)
            {
                playerLookController.SetIsViewingInventory(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Debug.LogWarning("PlayerLookController is null. Cannot set viewing inventory state.");
            }
        }
    }

    public static void Show()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(true);
            instance.UpdateInventoryUI();
            if (playerLookController != null)
            {
                playerLookController.SetIsViewingInventory(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Debug.LogWarning("PlayerLookController is null. Cannot set viewing inventory state.");
            }
        }
    }

    public static void Toggle()
    {
        if (instance != null)
        {
            if (instance.gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        else
        {
            Debug.LogWarning("InventoryUIController instance is null. Cannot toggle.");
        }
    }
}