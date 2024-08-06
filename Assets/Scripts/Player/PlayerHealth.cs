using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100;

    private static float currentHealth;
    
    // public AudioClip deathSound;

    public Slider healthSlider;
    bool isInvincible;

    public float invincibilityDuration;
    private float invincibilityCounter = 0;

    public AudioClip invincibleSound;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;

        if (invincibilityCounter > 0){
            invincibilityCounter -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage) {
        if (isInvincible && invincibilityCounter > 0){
            //do nothing
        } else {
            currentHealth -= damage;
            healthSlider.value = currentHealth;
            MakeVulnerable();
        }
        

        if (currentHealth <= 0) {
            Die();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("InvincibilityStar")) {
            isInvincible = true;
            invincibilityCounter = invincibilityDuration;
            AudioSource.PlayClipAtPoint(invincibleSound, Camera.main.transform.position);
            Destroy(other.gameObject);
        }
    }

    void MakeVulnerable(){
        isInvincible = false;
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

    public static float GetCurrentHealth()
    {
        return currentHealth;
    }

    public static void SetCurrentHealth(float health)
    {
        currentHealth = health;
        Debug.Log("Health: " + currentHealth);
    }
}
