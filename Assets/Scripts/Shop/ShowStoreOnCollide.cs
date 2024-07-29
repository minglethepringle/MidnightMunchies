using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStoreOnCollide : MonoBehaviour
{
    private GameObject shopUI;

    // Start is called before the first frame update
    void Start()
    {
        shopUI = GameObject.FindGameObjectWithTag("ShopUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            shopUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            shopUI.SetActive(false);
        }
    }
}
