using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNukes : MonoBehaviour
{
    public GameObject nukePrefab;
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
        InvokeRepeating("SpawnNuke", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnNuke(){
        Vector3 nukePosition;

        nukePosition.x = Random.Range(xMin, xMax);
        nukePosition.y = y;
        nukePosition.z = Random.Range(zMin, zMax);
        GameObject nuke = Instantiate(nukePrefab, nukePosition, transform.rotation) as GameObject;

        nuke.transform.parent = gameObject.transform;
    }
}
