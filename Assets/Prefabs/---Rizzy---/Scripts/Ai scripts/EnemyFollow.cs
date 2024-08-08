using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
    public string targetTag = "Player"; // Tag van het doelwit
    public float followRadius = 10f; // Maximale afstand om te volgen
    public float minDistance = 2f; // Minimale afstand om te stoppen met volgen
    public float normalSpeed = 3.5f; // Normale snelheid van de vijand
    public float increasedSpeed = 5f; // Verhoogde snelheid van de vijand
    public float outerRadius = 20f; // Buitenste radius voor verhoogde snelheid

    [Header("Attack Settings")]
    public Transform firePoint; // Punt waar de kogels vandaan komen (indien van toepassing)
    public GameObject bulletPrefab; // Prefab van de kogel (indien van toepassing)
    public EnemyAbility ability; // De ability van de vijand (aanval of genezing)
    public float abilityCooldown = 5f; // Cooldown tijd voor de ability

    private Transform target; // Doelwit transform
    private NavMeshAgent navAgent; // NavMeshAgent component
    private bool canUseAbility = true; // Kan de vijand een ability gebruiken

    void Start()
    {
        // Zoek het doelwit op basis van de tag
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            target = targetObject.transform;
        }

        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }
        else
        {
            navAgent.speed = normalSpeed; // Stel de snelheid in op de normale snelheid
        }
    }

    void Update()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > outerRadius)
            {
                navAgent.speed = increasedSpeed; // Verhoog de snelheid als buiten de buitenste radius
            }
            else
            {
                navAgent.speed = normalSpeed; // Normale snelheid als binnen de buitenste radius
            }

            if (distanceToTarget <= followRadius && distanceToTarget > minDistance)
            {
                // Beweeg richting het doelwit
                navAgent.SetDestination(target.position);
            }

            // Voer de ability uit als de vijand binnen de volg radius is en de cooldown is afgelopen
            if (distanceToTarget <= followRadius && canUseAbility)
            {
                StartCoroutine(PerformAbility());
            }
        }
    }

    IEnumerator PerformAbility()
    {
        canUseAbility = false;
        ability.ExecuteAbility(gameObject, target.gameObject, firePoint, bulletPrefab);
        yield return new WaitForSeconds(abilityCooldown); // Wacht voor de cooldown tijd
        canUseAbility = true;
    }

    void OnDrawGizmosSelected()
    {
        // Teken de follow radius, minimum distance radius en de outer radius in de Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
