using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private List<Transform> shopItems = new List<Transform>();
    private int selectedItemIndex = 0;

    private void Awake()
    {
        container = transform.Find("Container");
        shopItemTemplate = container.Find("ShopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton("Damage Item", Item.ItemType.Damage_1);
        CreateItemButton("Damage Item 2", Item.ItemType.Damage_2);

        UpdateSelection();
        Hide();
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItemIndex = (selectedItemIndex + 1) % shopItems.Count;
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItemIndex = (selectedItemIndex - 1 + shopItems.Count) % shopItems.Count;
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            TryToBuyItem((Item.ItemType)selectedItemIndex);
        }
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
        shopItems.Add(shopItemTransform);
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            Text itemName = shopItems[i].Find("ItemName").GetComponent<Text>();
            if (i == selectedItemIndex)
            {
                itemName.color = Color.white;
            }
            else
            {
                itemName.color = Color.gray;
            }
        }
    }

    private void TryToBuyItem(Item.ItemType item)
    {
        int cost = Item.GetCost(item);
        if (TrackHuskyDollars.GetCurrentBalance() >= cost)
        {
            TrackHuskyDollars.SubtractHuskyDollars(cost);
            Debug.Log("Item bought");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        selectedItemIndex = 0;
        UpdateSelection();
    }
}