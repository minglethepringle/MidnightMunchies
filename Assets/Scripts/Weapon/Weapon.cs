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

    public void Fire(Vector3 position, Quaternion rotation)
    {
        muzzleFlash.Play();

        Vector3 bulletStartingPosition = position + rotation * bulletOffset;
        GameObject projectile = Instantiate(projectilePrefab,
            bulletStartingPosition, // spawn at gun
            rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 twentyMetersForward = position + (rotation * Vector3.forward * 20);
        Vector3 bulletDirection = (twentyMetersForward - bulletStartingPosition).normalized;

        rb.AddForce(
            bulletDirection * projectileSpeed,
            ForceMode.VelocityChange);

        projectile.transform.SetParent(
            GameObject.FindGameObjectWithTag("ProjectileParent").transform);

        if (Random.Range(0, 2) == 0)
            AudioSource.PlayClipAtPoint(gunSFX1, position);
        else
            AudioSource.PlayClipAtPoint(gunSFX2, position);
    }
}
