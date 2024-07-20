using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackRate = 1.5f; // Seconds between attacks
    public float attackMoveSpeed = 0f; // Speed during attack
    public float chaseMoveSpeed = 3.5f; // Speed during chase

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float distanceToPlayer = Mathf.Infinity;
    private bool isProvoked = false;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    private enum State { Idle, Chasing, Attacking }
    private State currentState = State.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (isProvoked)
        {
            EngagePlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            isProvoked = true;
        }
    }

    private void EngagePlayer()
    {
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
        currentState = State.Chasing;
        animator.SetBool("isAttacking", false);
        animator.SetTrigger("Chase");
        navMeshAgent.speed = chaseMoveSpeed;
        navMeshAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        currentState = State.Attacking;
        isAttacking = true;
        navMeshAgent.speed = attackMoveSpeed;
        

        nextAttackTime = Time.time + attackRate;

        // Call the damage function once
        Invoke(nameof(PerformAttack), 0.5f); // Adjust the delay to match your animation timing
    }

    private void PerformAttack()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            animator.SetBool("isAttacking", true);
        }

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void Idle()
    {
        currentState = State.Idle;
        animator.SetBool("isAttacking", false);
        animator.ResetTrigger("Chase");
        navMeshAgent.ResetPath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
