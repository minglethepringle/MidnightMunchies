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
            
            Vector3 bulletStartingPosition = transform.right;
            GameObject projectile = Instantiate(projectilePrefab,
                transform.position,//bulletStartingPosition, // spawn at gun
                transform.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Vector3 hundredMetersForward = transform.forward * 100;
            // Vector3 bulletDirection = (hundredMetersForward - bulletStartingPosition).normalized;
            
            rb.AddForce(
                transform.forward * projectileSpeed,
                //bulletDirection * projectileSpeed, 
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
