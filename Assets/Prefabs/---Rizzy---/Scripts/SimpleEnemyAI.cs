using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    public float attackRate = 1.5f; // Seconds between attacks
    public int attackDamage = 10;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    private float distanceToPlayer = Mathf.Infinity;
    private float nextAttackTime = 0f;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    private void ChasePlayer()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
    }

    private void AttackPlayer()
    {
        navMeshAgent.isStopped = true;
        transform.LookAt(player);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isChasing", false);

        // Fire bullet towards player
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (player.position - firePoint.position).normalized * bulletSpeed;
        }

        nextAttackTime = Time.time + attackRate;
    }

    private void Idle()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
