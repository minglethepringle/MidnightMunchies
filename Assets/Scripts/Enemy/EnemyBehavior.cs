using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;

    public float speed = 0.02f;

    public float minimumDistance = 2f;

    public float damage = 20f;

    public AudioClip[] groanSounds;

    public AudioClip hitSound;

    private Animator animator;

    public float damageInterval = 1f;

    private AudioSource audioSource;
    private float lastDamageTime;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GroanRandomly());
    }

    private IEnumerator GroanRandomly()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            if (audioSource)
                audioSource.PlayOneShot(groanSounds[Random.Range(0, groanSounds.Length)]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (IsDead())
        {
            return;
        }

        if (CannotSeePlayer())
        {
            BecomeIdle();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > minimumDistance)
        {
            StartWalking();
            MoveTowardsPlayer();
        }
        else
        {
            StartAttacking();
        }
    }

    bool IsDead()
    {
        return animator.GetBool("dieNow");
    }

    bool CannotSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit))
        {
            if (hit.transform != player)
            {
                return true;
            }
        }

        return false;
    }

    void MoveTowardsPlayer()
    {

        // move towards the player but keep y at 0
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed
        );
        
        // keep y is 0
        transform.position = new Vector3(
            transform.position.x,
            0,
            transform.position.z
        );


        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    void StartWalking()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isIdle", false);
    }

    void StartAttacking()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", false);
    }

    void BecomeIdle()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
                lastDamageTime = Time.time;
            }
            // AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }
}
