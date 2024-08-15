using UnityEngine;

public class Airstrike : MonoBehaviour
{
    public GameObject missilePrefab;
    public float missileHeight = 50f;
    public float strikeRadius = 5f;
    public LayerMask groundLayer;
    public Material previewMaterial;
    public float damageRadius = 10f;
    public float explosionForce = 1000f;
    public GameObject explosionEffectPrefab;

    private GameObject previewCircle;
    private GameObject missileInstance;

    private void OnEnable()
    {
        CreatePreviewCircle();
    }

    private void OnDisable()
    {
        DestroyPreviewCircle();
    }

    private void Update()
    {
        UpdatePreviewCircle();

        if (Input.GetButtonDown("Fire1"))
        {
            LaunchMissile();
        }
    }

    private void CreatePreviewCircle()
    {
        previewCircle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        previewCircle.GetComponent<Collider>().enabled = false;
        previewCircle.transform.localScale = new Vector3(strikeRadius * 2, 0.01f, strikeRadius * 2);
        previewCircle.GetComponent<Renderer>().material = previewMaterial;
    }

    private void UpdatePreviewCircle()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            previewCircle.transform.position = hit.point;
            previewCircle.transform.up = hit.normal;
        }
    }

    private void DestroyPreviewCircle()
    {
        if (previewCircle != null)
        {
            Destroy(previewCircle);
        }
    }

    private void LaunchMissile()
    {
        Vector3 strikePosition = previewCircle.transform.position;
        Vector3 missileStartPosition = strikePosition + Vector3.up * missileHeight;

        missileInstance = Instantiate(missilePrefab, missileStartPosition, Quaternion.identity);
        missileInstance.transform.LookAt(strikePosition);

        Rigidbody missileRb = missileInstance.GetComponent<Rigidbody>();
        if (missileRb == null)
        {
            missileRb = missileInstance.AddComponent<Rigidbody>();
        }
        missileRb.useGravity = false;
        missileRb.velocity = (strikePosition - missileStartPosition).normalized * 300f;

        missileInstance.AddComponent<MissileImpact>().Initialize(this);
    }

    public void CreateExplosion(Vector3 position)
    {
        Debug.Log("Airstrike: CreateExplosion");
        // Create explosion effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, position, Quaternion.identity);
        }

        // Detect and damage nearby objects
        Collider[] colliders = Physics.OverlapSphere(position, damageRadius);
        foreach (Collider hit in colliders)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(PlayerWeaponManager.CurrentWeaponDamage());
            }

            // Apply explosion force to rigidbodies
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, position, damageRadius);
            }
        }
    }
}

public class MissileImpact : MonoBehaviour
{
    private Airstrike airstrikeController;

    public void Initialize(Airstrike controller)
    {
        airstrikeController = controller;
    }

    private void OnCollisionEnter(Collision collision)
    {
        airstrikeController.CreateExplosion(transform.position);
        Destroy(gameObject);
    }
}