using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRegularMoney : MonoBehaviour
{
    public GameObject moneyPrefab;
    public float xMin = -25;
    public float xMax = 25;

    //public float yMin = 8;
    //public float yMax = 25;

    public float y = 0;

    public float zMin = -25;
    public float zMax = 25;

    public float spawnTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMoney", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnMoney(){
        Vector3 moneyPosition;

        moneyPosition.x = Random.Range(xMin, xMax);
        moneyPosition.y = y;
        moneyPosition.z = Random.Range(zMin, zMax);
        GameObject money = Instantiate(moneyPrefab, moneyPosition, transform.rotation) as GameObject;

        money.transform.parent = gameObject.transform;
    }
}
