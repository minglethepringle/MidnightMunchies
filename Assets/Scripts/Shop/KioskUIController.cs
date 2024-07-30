using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KioskUIController : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private static PlayerLookController playerLookController;
    private static KioskUIController instance;

    private Text alert;

    private Dictionary<Item.ItemType, GameObject> shopItems = new Dictionary<Item.ItemType, GameObject>();

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
        shopItemTemplate = container.Find("ShopItemTemplate");
        
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
            Debug.LogWarning("Alert object not found in ShopUIController.");
        }
    }

    private void Start()
    {
        if (playerLookController == null)
        {
            playerLookController = Camera.main.GetComponent<PlayerLookController>();
        }

        CreateItemButton(Item.ItemType.Subway1);
        CreateItemButton(Item.ItemType.Subway2);
        CreateItemButton(Item.ItemType.Subway3);
        CreateItemButton(Item.ItemType.Subway4);
        CreateItemButton(Item.ItemType.Subway5);
        CreateItemButton(Item.ItemType.Subway6);
        CreateItemButton(Item.ItemType.Subway7);
        CreateItemButton(Item.ItemType.Subway8);

        Hide();
    }

    private void CreateItemButton(Item.ItemType itemType)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(
            -50,
            60 + (-shopItemHeight * container.childCount)
        );

        Text itemName = shopItemTransform.Find("ItemName").GetComponent<Text>();
        Text itemPrice = shopItemTransform.Find("ItemPrice").GetComponent<Text>();
        Text itemSold = shopItemTransform.Find("ItemSold").GetComponent<Text>();

        itemName.text = Item.GetName(itemType);
        itemPrice.text = '$' + Item.GetCost(itemType).ToString();
        itemSold.text = "SOLD";

        itemSold.gameObject.SetActive(false);

        shopItemTransform.gameObject.SetActive(true);

        Button button = shopItemTransform.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            TryToBuyItem(itemType);
        });

        shopItems[itemType] = shopItemTransform.gameObject;

        UpdateItemUI(itemType);
    }

    private void TryToBuyItem(Item.ItemType itemType)
    {
        if (Item.IsItemPurchased(itemType))
        {
            SetAlertText("Item already purchased");
            return;
        }

        int cost = Item.GetCost(itemType);
        if (PlayerBankAccount.GetCurrentBalance() >= cost)
        {
            PlayerBankAccount.SubtractHuskyDollars(cost);
            Item.PurchaseItem(itemType);
            SetAlertText("Item bought: " + Item.GetName(itemType));
            UpdateItemUI(itemType);
        }
        else
        {
            SetAlertText("Not enough money");
        }
    }

    private void UpdateItemUI(Item.ItemType itemType)
    {
        if (shopItems.TryGetValue(itemType, out GameObject shopItem))
        {
            Text itemPrice = shopItem.transform.Find("ItemPrice").GetComponent<Text>();
            Text itemSold = shopItem.transform.Find("ItemSold").GetComponent<Text>();

            if (Item.IsItemPurchased(itemType))
            {
                itemPrice.gameObject.SetActive(false);
                itemSold.gameObject.SetActive(true);
            }
            else
            {
                itemPrice.gameObject.SetActive(true);
                itemSold.gameObject.SetActive(false);
            }
        }
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
                playerLookController.SetIsShopping(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Debug.LogWarning("PlayerLookController is null. Cannot set shopping state.");
            }
        }
    }

    public static void Show()
    {
        if (instance != null)
        {
            instance.gameObject.SetActive(true);
            if (playerLookController != null)
            {
                playerLookController.SetIsShopping(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Debug.LogWarning("PlayerLookController is null. Cannot set shopping state.");
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
            Debug.LogWarning("ShopUIController instance is null. Cannot toggle.");
        }
        
    }
}