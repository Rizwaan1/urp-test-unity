using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    private NavMeshAgent navMeshAgent;
    private RCUAttack rcuAttack;

    private float distanceToPlayer = Mathf.Infinity;
    public float chaseRange = 10f;
    public float stopRange = 7f;

    public bool isFrozen = false;
    public bool isAggro = false;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rcuAttack = GetComponent<RCUAttack>();
    }

    void Update()
    {
        AggroSystem();
        distanceToPlayer = Vector3.Distance(playerTarget.position, transform.position);
        if (!isFrozen)
        {
            if (distanceToPlayer <= chaseRange)
            {
                isAggro = true;
                aggroCurrentTime = 0;

                Debug.Log("Within ChaseRange");
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(playerTarget.position);

                if(distanceToPlayer <= stopRange)
                {
                    navMeshAgent.isStopped = true;
                    Debug.Log("Within StopRange");
                }
                else
                    navMeshAgent.isStopped = false;
            }
        }        
    }

    //Aggro Variables
    [SerializeField] private float aggroTime = 4;
    [SerializeField] private float aggroCurrentTime = 0;
    private void AggroSystem()
    {
        AggroTimer();
        if (isAggro)
        {
            rcuAttack.firingState = RCUAttack.FiringState.FireVolley;           
                navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(playerTarget.position);
        }
        else
        {
            rcuAttack.firingState = RCUAttack.FiringState.Idle;
            navMeshAgent.isStopped = true;
        }           
    }

    private void AggroTimer()
    {
        if(isAggro)
        {
            if(aggroCurrentTime >= aggroTime)
            {
                isAggro = false;
                aggroCurrentTime = 0;
            }
            else
            {
                aggroCurrentTime += Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopRange);
    }
}
