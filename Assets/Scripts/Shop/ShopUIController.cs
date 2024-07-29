using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;

    private static PlayerLookController playerLookController;
    private static ShopUIController instance;

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
    }

    private void Start()
    {
        if (playerLookController == null)
        {
            playerLookController = Camera.main.GetComponent<PlayerLookController>();
        }

        CreateItemButton("Damage Item", Item.ItemType.Damage_1);
        Hide();
    }

    private void CreateItemButton(string name, Item.ItemType itemType)
    {
        int price = Item.GetCost(itemType);

        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(
            -50,
            60 + (-shopItemHeight * container.childCount)
        );

        Text itemName = shopItemTransform.Find("ItemName").GetComponent<Text>();
        Text itemPrice = shopItemTransform.Find("ItemPrice").GetComponent<Text>();
        itemName.text = name;
        itemPrice.text = '$' + price.ToString();

        shopItemTransform.gameObject.SetActive(true);

        Button button = shopItemTransform.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            TryToBuyItem(itemType);
        });
    }

    private void TryToBuyItem(Item.ItemType item)
    {
        int cost = Item.GetCost(item);
        if (PlayerBankAccount.GetCurrentBalance() >= cost)
        {
            PlayerBankAccount.SubtractHuskyDollars(cost);
            Debug.Log("Item bought");
        }
        else
        {
            Debug.Log("Not enough money");
        }
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
                Cursor.lockState = CursorLockMode.Locked;
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