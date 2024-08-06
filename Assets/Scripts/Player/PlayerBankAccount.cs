using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBankAccount : MonoBehaviour
{
    public int startingBalance = 100;

    public static Text balanceText;
    public static Text balanceTextShadow;

    public AudioClip moneySound;
    private static AudioClip moneySoundStatic;

    private static int currentBalance;

    // Start is called before the first frame update
    void Start()
    {
        currentBalance = startingBalance;
        moneySoundStatic = moneySound;

        GameObject[] objects = GameObject.FindGameObjectsWithTag("MoneyUI");
        balanceText = objects[0].GetComponent<Text>();
        balanceTextShadow = objects[1].GetComponent<Text>();
        UpdateBalanceText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Money")) {
            AddHuskyDollars(10);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Moneyx2")) {
            AddHuskyDollars(20);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Money1Dollar")) {
            AddHuskyDollars(1);
            Destroy(other.gameObject);
        }
    }

    public static void AddHuskyDollars(int amount) {
        if (moneySoundStatic)
        {
            AudioSource.PlayClipAtPoint(moneySoundStatic, Camera.main.transform.position);
        }
        currentBalance += amount;
        UpdateBalanceText();
    }

    public static void SubtractHuskyDollars(int amount) {
        currentBalance -= amount;
        UpdateBalanceText();
    }

    public static int GetCurrentBalance() {
        return currentBalance;
    }

    public static void SetCurrentBalance(int balance)
    {
        currentBalance = balance;
        Debug.Log("balance: " + currentBalance);
    }

    public static void UpdateBalanceText() {
        balanceText.text = "$" + currentBalance;
        balanceTextShadow.text = "$" + currentBalance;
    }
}
