using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float attackDamage = 20f;
    public float attackRange = 1.5f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;

    private float lastAttackTime;
    private Transform player;
    private PlayerHealth playerHealth;
    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the player object.");
        }

        if (agent != null)
        {
            agent.updateRotation = true;  // Zorg ervoor dat de agent roteert naar de bewegingsrichting
            agent.updatePosition = true;  // Zorg ervoor dat de agent de positie bijwerkt
            agent.speed = walkSpeed;  // Start met loopsnelheid
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                ChasePlayer(distanceToPlayer);
            }
            else
            {
                StopChasingPlayer();
            }
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        agent.SetDestination(player.position);

        if (distanceToPlayer > attackRange)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);

            agent.speed = runSpeed;  // Ren naar de speler als hij buiten het aanvalsbereik is
        }
        else
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void StopChasingPlayer()
    {
        agent.ResetPath();
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;  // Stop de beweging tijdens de aanval
        animator.SetBool("isAttacking", true);
        if (playerHealth != null)
        {
            playerHealth.ApplyDamagezombie(attackDamage);
            Debug.Log("Zombie attacked player. Damage: " + attackDamage);
        }
        agent.isStopped = false;  // Hervat de beweging na de aanval
    }

    void Die()
    {
        Debug.Log("Zombie died!");
        animator.SetTrigger("Die");

        agent.enabled = false;  // Schakel de NavMeshAgent uit bij de dood

        // Voeg ragdoll effect toe door de kinematic van alle child rigidbodies uit te schakelen
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        this.enabled = false;  // Schakel het script uit om verdere bewegingen te stoppen
    }
}
