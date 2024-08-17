using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PistolPickupBehavior : MonoBehaviour
{
    public Text hintText;
    public Transform player;
    public AudioClip pickupSFX;
    private bool isLooking;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooking && Input.GetKeyDown(Controls.PICKUP_WEAPON))
        {
            Inventory.AddItem(Item.ItemType.Pistol);
            PlayerWeaponManager.SwitchToPistol();
            hintText.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(pickupSFX, player.position, 0.15f);
            gameObject.SetActive(false);
            Destroy(gameObject, 2);
        }
    }
    
    private void OnMouseOver()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < 5)
        {
            hintText.gameObject.SetActive(true);
            isLooking = true;
        }
        else
        {
            hintText.gameObject.SetActive(false);
            isLooking = false;
        }
    }
    
    private void OnMouseExit()
    {
        hintText.gameObject.SetActive(false);
        isLooking = false;
    }
}
