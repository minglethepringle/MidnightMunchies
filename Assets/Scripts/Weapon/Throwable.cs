using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float throwForce = 10f;
    public float throwUpwardForce = 5f;
    public GameObject explosionEffectPrefab;
    public float explosionRadius = 5f;
    public float explosionForce = 500f;
    public float damageRadius = 5f;

    private bool hasExploded = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        rb.isKinematic = false;
        Vector3 throwDirection = transform.forward;
        rb.AddForce(throwDirection * throwForce + Vector3.up * throwUpwardForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        hasExploded = true;

        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Detect and damage nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider hit in colliders)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(PlayerWeaponManager.CurrentWeaponDamage());
            }

            // // Apply explosion force to rigidbodies
            // Rigidbody rb = hit.GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            // }
        }

        ResetPosition();
    }

    private void ResetPosition()
    {
        hasExploded = false;
        rb.isKinematic = true;
        transform.localPosition = new Vector3(0.22f, -0.22f, 0.35f);
    }
}