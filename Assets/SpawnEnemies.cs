using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
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
        InvokeRepeating("SpawnZombies", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnZombies(){
        Vector3 enemyPosition;

        enemyPosition.x = Random.Range(xMin, xMax);
        enemyPosition.y = y;
        enemyPosition.z = Random.Range(zMin, zMax);
        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation) as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
    }
}
