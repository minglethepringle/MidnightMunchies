using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCheckpoint : MonoBehaviour
{
    public static int balance = 0;
    public static float health = 100;
    public static HashSet<Item.ItemType> itemsPurchased;
    public static Dictionary<Item.ItemType, int> inventoryItems;
    public static Dictionary<int, Item.ItemType> slotItems;
    public static Vector3 location;
    public static Quaternion rotation;
    public static int levelIndex, objectiveIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void RecordCheckpoint()
    {
        Debug.Log("record checkpoint");
        balance = PlayerBankAccount.GetCurrentBalance();
        health = PlayerHealth.GetCurrentHealth();
        itemsPurchased = Item.GetItemsPurchased();
        inventoryItems = Inventory.instance.inventoryItems;
        slotItems = InventoryUIController.slotItems;
        location = GameObject.FindGameObjectWithTag("Player").transform.position;
        rotation = GameObject.FindGameObjectWithTag("Player").transform.rotation;
        levelIndex = LevelManager.levelIndex;
        objectiveIndex = LevelManager.currentObjectiveIndex;
    }

    public static void RevertToCheckpoint()
    {
        Debug.Log("revert called");
        PlayerHealth.SetCurrentHealth(health);
        PlayerHealth.Revive();

        PlayerBankAccount.SetCurrentBalance(balance);
        PlayerBankAccount.UpdateBalanceText();
        Item.SetItemsPurchased(itemsPurchased);
        Inventory.instance.inventoryItems = inventoryItems;
        InventoryUIController.slotItems = slotItems;
        // If they have at least the pistol, switch to it
        if (inventoryItems.Count > 1)
            PlayerWeaponManager.SwitchToPistol();
        
        LevelManager.levelIndex = levelIndex;
        LevelManager.currentObjectiveIndex = objectiveIndex;

        // Destroy and respawn enemies
        LevelManager.staticLevelSpawners[levelIndex].GetComponent<EnemySpawner>().Purge();
        LevelManager.SpawnLevelEnemies();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = location;
        player.transform.rotation = rotation;

        PlayerMovementController.locked = false;
        PlayerLookController.locked = false;
    }
}
