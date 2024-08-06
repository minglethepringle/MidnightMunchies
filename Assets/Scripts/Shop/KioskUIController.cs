using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

/**
 * IMPORTANT: Attach this to every individual kiosk UI object as each kiosk will have its own UI.
 */
public class KioskUIController : MonoBehaviour
{
    public Button[] itemButtons;
    public Button orderButton;
    public GameObject cartContent;
    public GameObject cartItemPrefab;
    public Text totalText;
    public AudioClip orderSound;
    
    private int total = 0;

    private void Start()
    {
        foreach (Button itemButton in itemButtons)
        {
            // Get the item name from the button text
            string itemName = itemButton.GetComponentInChildren<Text>().text;
            itemButton.onClick.AddListener(() => AddToCart(itemName));
        }
        
        orderButton.onClick.AddListener(PlaceOrder);
    }
    
    private void AddToCart(string itemName)
    {
        // Parse out name and price
        string name = itemName.Split(":")[0].Trim();
        int price = int.Parse(itemName.Split("$")[1].Trim());
        
        // Add cart item to visual scroll view
        GameObject cartItem = Instantiate(cartItemPrefab, cartContent.transform);
        cartItem.GetComponentInChildren<Text>().text = name;
        
        // Update total
        total += price;
        totalText.text = "$" + total;
    }

    private void PlaceOrder()
    {
        PlayerBankAccount.SubtractHuskyDollars(total);
        
        // Clear cart
        foreach (Transform child in cartContent.transform)
        {
            Destroy(child.gameObject);
        }
        
        total = 0;
        totalText.text = "$" + total;

        PlayerCheckpoint.RecordCheckpoint();
        
        AudioSource.PlayClipAtPoint(orderSound, Camera.main.transform.position);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}