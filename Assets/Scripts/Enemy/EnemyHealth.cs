using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float startingHealth = 100;

    private float currentHealth;

    public float projectileDamage = 10f;

    public Slider healthSlider;

    public GameObject moneyPrefab;

    private Animator animator;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();

        healthSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            return;
        }

        if (currentHealth < startingHealth && CalculateDistanceFromPlayer() < 20)
        {
            healthSlider.gameObject.SetActive(true);
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (isDead) {
            return;
        }
        
        if (other.CompareTag("Bullet")) {
            TakeDamage(projectileDamage);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        isDead = true;
        animator.SetBool("dieNow", true);
        Destroy(healthSlider.gameObject);
        Destroy(gameObject.GetComponent<BoxCollider>());

        Vector3 moneyPosition = transform.position;
        moneyPosition.y = 1;
        Instantiate(moneyPrefab, moneyPosition, Quaternion.identity);
    }

    private float CalculateDistanceFromPlayer() {
        GameObject player = GameObject.FindWithTag("Player");
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance;
    }
}
