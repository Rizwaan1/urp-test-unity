using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2.0f;
    public float damage = 10.0f;
    public float attackRate = 1.0f; // Aanvallen per seconde
    public float movementSpeed = 3.5f; // Bewegingssnelheid van de vijand

    private NavMeshAgent navMeshAgent;
    private bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = movementSpeed; // Stel de bewegingssnelheid in
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                navMeshAgent.SetDestination(player.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        while (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy attacks the player!");
            }

            yield return new WaitForSeconds(1.0f / attackRate); // Attack rate
        }

        isAttacking = false;
    }
}
