using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyAI : MonoBehaviour
{
    public Transform target; // Het doelwit, bijvoorbeeld de speler
    public float speed = 5f; // De snelheid waarmee de vijand beweegt
    public float heightOffset = 3.0f; // De hoogte waarop de vijand moet zweven
    public float stopDistance = 10f; // De afstand waarop de vijand stopt en begint te schieten
    public float shootInterval = 1.5f; // De tijd tussen schoten
    public GameObject projectilePrefab; // De kogel of projectiel dat de vijand afvuurt
    public Transform shootPoint; // Het punt waaruit het projectiel wordt afgevuurd

    private NavMeshAgent agent;
    private float lastShootTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        lastShootTime = -shootInterval; // Zodat de vijand meteen kan schieten bij het eerste contact
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > stopDistance)
            {
                // Beweeg naar het doelwit als we te ver weg zijn
                MoveTowardsTarget();
            }
            else
            {
                // Stop met bewegen en begin te schieten
                agent.velocity = Vector3.zero; // Stop beweging
                LookAtTarget();
                ShootAtTarget();
            }
        }
    }

    void MoveTowardsTarget()
    {
        // Bereken de richting naar het doelwit
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Zorg ervoor dat de vijand alleen horizontaal beweegt

        // Beweeg de vijand met de opgegeven snelheid
        Vector3 movement = direction * speed * Time.deltaTime;
        agent.nextPosition = transform.position + movement;

        // Stel de hoogte van de vijand in
        Vector3 newPosition = agent.nextPosition;
        newPosition.y = target.position.y + heightOffset;
        agent.Warp(newPosition); // Verplaats de agent naar de nieuwe positie
    }

    void LookAtTarget()
    {
        // Laat de vijand naar het doelwit kijken, inclusief hoogte
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

    void ShootAtTarget()
    {
        // Controleer of de tijd is om te schieten
        if (Time.time > lastShootTime + shootInterval)
        {
            lastShootTime = Time.time;

            // Instantieer het projectiel en richt het direct op het doelwit
            if (projectilePrefab != null && shootPoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    // Schiet het projectiel in de richting van de speler, inclusief de hoogteverschil
                    Vector3 direction = (target.position - shootPoint.position).normalized;
                    rb.velocity = direction * speed;
                }
            }
        }
    }
}
