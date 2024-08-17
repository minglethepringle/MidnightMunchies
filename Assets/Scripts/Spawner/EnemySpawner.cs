using System;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public PolyShape polyShape;
    public float numEnemies = 10;

    private MeshCollider polyShapeCollider;

    // Start is called before the first frame update
    void Start()
    {
        try 
        {
            polyShapeCollider = polyShape.GetComponent<MeshCollider>();
        }
        catch (Exception e)
        {
            Debug.LogError($"Encountered exception: {e.Message}");
            Debug.LogError("PolyShape does not have a MeshCollider component. Trying to find one in children.");
            polyShapeCollider = polyShape.GetComponentInChildren<MeshCollider>();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 randomPosition = GetRandomWorldPointInPolyShape();
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomPosition, randomRotation);
            spawnedEnemy.transform.parent = gameObject.transform;
        }
    }

    public void Purge()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Vector3 GetRandomWorldPointInPolyShape()
    {
        // Get the bounds of the mesh
        Bounds worldBounds = polyShapeCollider.bounds;

        // Loop until a valid point is found
        Vector3 worldPoint;
        do
        {
            float randomX = Random.Range(worldBounds.min.x, worldBounds.max.x);
            float randomZ = Random.Range(worldBounds.min.z, worldBounds.max.z);
            worldPoint = new Vector3(randomX, 0, randomZ);
        } while (!IsPointInPolyShape(worldPoint));

        return worldPoint;
    }

    private bool IsPointInPolyShape(Vector3 point)
    {
        // Raycast from this point (slightly above) downward to see if it hits the collider
        Ray ray = new Ray(point + Vector3.up * 0.1f, Vector3.down);
        return polyShapeCollider.Raycast(ray, out _, 1000f);
    }
}
