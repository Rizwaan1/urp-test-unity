using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    public Transform player; // Sleep hier de speler in vanuit de editor
    public float health = 100f;
    public float attackRange = 2f;
    public float damage = 10f;
    public float attackCooldown = 1f; // Tijd tussen aanvallen
    public float stoppingDistance = 1.5f; // Afstand waarop de zombie stopt voor de speler

    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;
    private float lastAttackTime;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastAttackTime = -attackCooldown; // Zorgt ervoor dat de zombie direct kan aanvallen

        // Stel de stopping distance in voor de NavMeshAgent
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // De zombie stopt voor de speler en valt aan als hij binnen attack range is
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        // Hier zou je de speler schade toebrengen
        Debug.Log("Zombie valt aan en doet " + damage + " schade!");

        // Voorbeeld om de health van de speler aan te passen, zorg ervoor dat de speler een script heeft met een health variabele
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        lastAttackTime = Time.time; // Update de tijd van de laatste aanval
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Voer doodseffecten uit (bijv. animatie, verwijderen van object, etc.)
        Debug.Log("Zombie is dood!");
        Destroy(gameObject);
    }
}
