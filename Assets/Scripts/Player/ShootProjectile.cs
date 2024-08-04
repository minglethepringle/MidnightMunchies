using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{

    public GameObject projectilePrefab;
    public float projectileSpeed = 100;
    public AudioClip gunSFX1;
    public AudioClip gunSFX2;

    public Image reticleImage;
    public Color reticleEnemyColor;

    Color originalReticleColor;

    public ParticleSystem muzzleFlash;
    

    // Start is called before the first frame update
    void Start()
    {
        originalReticleColor = reticleImage.color;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            muzzleFlash.Play();
            
            Vector3 bulletStartingPosition = transform.position + (transform.right * 0.25f);
            GameObject projectile = Instantiate(projectilePrefab,
                bulletStartingPosition, // spawn at gun
                transform.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            Vector3 twentyMetersForward = transform.position + (transform.forward * 20);
            Vector3 bulletDirection = (twentyMetersForward - bulletStartingPosition).normalized;

            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            // Vector3 final_target = target + (Vector3.right * 0.25f);
            
            rb.AddForce(
                target * projectileSpeed,
                // bulletDirection * projectileSpeed, 
                ForceMode.VelocityChange);

            projectile.transform.SetParent(
                GameObject.FindGameObjectWithTag("ProjectileParent").transform);

            if (Random.Range(0, 2) == 0)
                AudioSource.PlayClipAtPoint(gunSFX1, transform.position);
            else
                AudioSource.PlayClipAtPoint(gunSFX2, transform.position);
        }
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                reticleImage.color = Color.Lerp
                    (reticleImage.color, reticleEnemyColor, Time.deltaTime * 2);
                reticleImage.transform.localScale = Vector3.Lerp(
                    reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1),
                    Time.deltaTime * 2);
            }
            else
            {
                reticleImage.color = Color.Lerp
                    (reticleImage.color, originalReticleColor, Time.deltaTime * 2);
                reticleImage.transform.localScale = Vector3.Lerp(
                    reticleImage.transform.localScale, Vector3.one,
                    Time.deltaTime * 2);
            }
        }
    }
}
