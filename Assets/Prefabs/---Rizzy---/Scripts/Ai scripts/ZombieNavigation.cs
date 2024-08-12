using UnityEngine;
using UnityEngine.AI;

public class ZombieNavigation : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public Transform[] waypoints;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            return; // Stop het script als dit ontbreekt
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player object is not found in the scene. Make sure the player object is tagged as 'Player'.");
            return; // Stop het script als de speler niet gevonden wordt
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned in the Inspector.");
            return; // Stop het script als er geen waypoints zijn
        }

        NavigateToRandomWaypoint();
    }

    void Update()
    {
        if (Vector3.Distance(agent.destination, transform.position) < 1f) // Als zombie waypoint bereikt heeft
        {
            NavigateToPlayer();
        }
    }

    void NavigateToRandomWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }

    void NavigateToPlayer()
    {
        agent.SetDestination(player.position);
    }
}
