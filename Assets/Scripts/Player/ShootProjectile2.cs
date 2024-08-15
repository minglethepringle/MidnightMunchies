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

    public Throwable currentThrowable;

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
        if (!PlayerWeaponManager.currentWeapon) return;

        Weapon currentWeapon = PlayerWeaponManager.currentWeapon.GetComponent<Weapon>();
        Throwable currentThrowable = PlayerWeaponManager.currentWeapon.GetComponent<Throwable>();

        if (currentWeapon != null && currentWeapon.isActiveAndEnabled)
        {
            HandleWeaponFiring(currentWeapon);
        }
        else if (currentThrowable != null && currentThrowable.isActiveAndEnabled)
        {
            HandleThrowableThrowing(currentThrowable);
        }
    }

    private void HandleWeaponFiring(Weapon weapon)
    {
        if (weapon.hasContinousFire)
        {
            if (Input.GetButton("Fire1") && Time.time - lastFireTime >= ASSAULT_RIFLE_FIRE_RATE)
            {
                weapon.Fire(transform.position, transform.rotation);
                lastFireTime = Time.time;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.Fire(transform.position, transform.rotation);
            }
        }
    }

    private void HandleThrowableThrowing(Throwable throwable)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            throwable.Throw();
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