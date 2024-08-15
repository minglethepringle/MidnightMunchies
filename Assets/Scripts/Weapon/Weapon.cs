using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 100;
    public AudioClip gunSFX1;
    public AudioClip gunSFX2;
    public Vector3 bulletOffset = new Vector3(0.2f, -0.1f, 0);
    public ParticleSystem muzzleFlash;
    public bool hasContinousFire = false;
    public bool hasSpread = false;
    public int spreadProjectileCount = 5;
    public float spreadAngle = 20f;

    public void Fire(Vector3 position, Quaternion rotation)
    {
        muzzleFlash.Play();

        if (hasSpread)
        {
            FireWithSpread(position, rotation);
        }
        else
        {
            FireSingleProjectile(position, rotation);
        }

        PlayFireSound(position);
    }

    private void FireSingleProjectile(Vector3 position, Quaternion rotation)
    {
        Vector3 bulletStartingPosition = position + rotation * bulletOffset;
        GameObject projectile = Instantiate(projectilePrefab, bulletStartingPosition, rotation);
        SetProjectileVelocity(projectile, position, rotation, bulletStartingPosition);
    }

    private void FireWithSpread(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < spreadProjectileCount; i++)
        {
            Vector3 bulletStartingPosition = position + rotation * bulletOffset;
            Quaternion spreadRotation = CalculateSpreadRotation(rotation);
            GameObject projectile = Instantiate(projectilePrefab, bulletStartingPosition, spreadRotation);
            SetProjectileVelocity(projectile, position, spreadRotation, bulletStartingPosition);
        }
    }

    private Quaternion CalculateSpreadRotation(Quaternion baseRotation)
    {
        float randomSpreadX = Random.Range(-spreadAngle, spreadAngle);
        float randomSpreadY = Random.Range(-spreadAngle, spreadAngle);
        return baseRotation * Quaternion.Euler(randomSpreadX, randomSpreadY, 0);
    }

    private void SetProjectileVelocity(GameObject projectile, Vector3 position, Quaternion rotation, Vector3 bulletStartingPosition)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 twentyMetersForward = position + (rotation * Vector3.forward * 20);
        Vector3 bulletDirection = (twentyMetersForward - bulletStartingPosition).normalized;

        rb.AddForce(bulletDirection * projectileSpeed, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

    private void PlayFireSound(Vector3 position)
    {
        AudioClip soundToPlay = Random.Range(0, 2) == 0 ? gunSFX1 : gunSFX2;
        AudioSource.PlayClipAtPoint(soundToPlay, position);
    }
}