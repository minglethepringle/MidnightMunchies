using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInvincibility : MonoBehaviour
{
    public GameObject invincibilityPrefab;
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
        InvokeRepeating("SpawnInvincibile", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnInvincibile(){
        Vector3 invincibilityPosition;

        invincibilityPosition.x = Random.Range(xMin, xMax);
        invincibilityPosition.y = y;
        invincibilityPosition.z = Random.Range(zMin, zMax);
        GameObject invincibility = Instantiate(invincibilityPrefab, invincibilityPosition, transform.rotation) as GameObject;

        invincibility.transform.parent = gameObject.transform;
    }
}
