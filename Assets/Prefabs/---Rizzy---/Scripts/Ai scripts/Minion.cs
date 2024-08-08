using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    public float lifeTime = 30f; // Hoe lang de minion leeft
    public float explosionRange = 2f; // Afstand waarop de minion explodeert
    public float explosionDamage = 50f; // Schade van de explosie
    public string[] targetTags; // Tags van mogelijke doelwitten (bijv. "Player", "Enemy")
    public GameObject explosionEffect; // Prefab voor het explosie-effect

    private Transform target; // Doelwit van de minion
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Vernietig de minion na een bepaalde tijd
        navMeshAgent = GetComponent<NavMeshAgent>();
        FindClosestTarget(); // Zoek direct naar het dichtstbijzijnde doelwit
    }

    void Update()
    {
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position); // Stel het doel in voor de NavMeshAgent

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= explosionRange)
            {
                Explode(); // Voer de explosie uit
            }
        }
    }

    void FindClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (string tag in targetTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, potentialTarget.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = potentialTarget.transform;
                }
            }
        }

        target = closestTarget;
    }

    void Explode()
    {
        // Creëer het explosie-effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Toebrengen van schade aan het doelwit
        if (target.CompareTag("Player"))
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explosionDamage);
            }
        }
        else if (target.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(explosionDamage);
            }
        }

        // Vernietig de minion na de explosie
        Destroy(gameObject);
    }
}
