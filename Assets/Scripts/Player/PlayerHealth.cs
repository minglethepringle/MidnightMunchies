using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100;

    private float currentHealth;
    
    // public AudioClip deathSound;

    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        // AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // rotate the object smoothly so it is lying flat facing up:
        // lerp
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(90, 0, 0),
            Time.deltaTime
        );
        FindObjectOfType<LevelManager>().LevelLost();

        // Destroy(gameObject, );
    }
}
