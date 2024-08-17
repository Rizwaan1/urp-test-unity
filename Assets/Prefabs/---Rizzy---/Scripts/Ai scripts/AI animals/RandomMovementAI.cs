using UnityEngine;
using UnityEngine.AI;

public class RandomMovementAI : MonoBehaviour
{
    public float wanderRadius = 10f;  // De straal waarin de AI kan rondlopen
    public float minWanderTimer = 2f; // Minimale tijd voordat de AI een nieuwe bestemming kiest
    public float maxWanderTimer = 5f; // Maximale tijd voordat de AI een nieuwe bestemming kiest

    private NavMeshAgent agent;
    private float timer;
    private float currentWanderTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetRandomWanderTimer();
        timer = currentWanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentWanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            SetRandomWanderTimer();
            timer = 0;
        }
    }

    void SetRandomWanderTimer()
    {
        currentWanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
