using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile2 : MonoBehaviour
{

    public Image reticleImage;
    public Color reticleEnemyColor;

    Color originalReticleColor;

    public Weapon currentWeapon;

    private float lastFireTime = 0f;

    private const float ASSAULT_RIFLE_FIRE_RATE = 0.2f; // 5 shots per second (1 / 5 = 0.2 seconds between shots)


    // Start is called before the first frame update
    void Start()
    {
        originalReticleColor = reticleImage.color;

        currentWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLookController.locked || PlayerMovementController.locked) return;
        
        currentWeapon = PlayerWeaponManager.currentWeapon.GetComponent<Weapon>();

        if (currentWeapon != null && currentWeapon.isActiveAndEnabled)
        {
            if (currentWeapon.hasContinousFire)
            {
                // Continuous firing for AssaultRifle when Fire1 is held down, with fire rate limit
                if (Input.GetButton("Fire1"))
                {
                    if (Time.time - lastFireTime >= ASSAULT_RIFLE_FIRE_RATE)
                    {
                        currentWeapon.Fire(transform.position, transform.rotation);
                        lastFireTime = Time.time;
                    }
                }
            }
            else
            {
                // Single shot for other weapons when Fire1 is pressed
                if (Input.GetButtonDown("Fire1"))
                {
                    currentWeapon.Fire(transform.position, transform.rotation);
                }
            }
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
                    (reticleImage.color, reticleEnemyColor, Time.deltaTime * 5);
            }
            else
            {
                reticleImage.color = Color.Lerp
                    (reticleImage.color, originalReticleColor, Time.deltaTime * 5);
            }
        }
    }
}