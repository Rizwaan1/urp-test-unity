using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCUAttack : MonoBehaviour
{

    public enum FiringState
    {
        Idle = 1,
        FireVolley
    }

    public FiringState firingState;
    void Start()
    {

    }

    void Update()
    {
        switch (firingState)
        {
            case FiringState.Idle: break;

            case FiringState.FireVolley: break;

            default:
                firingState = FiringState.Idle; break;
        }
    }

    private void IdleState()
    {
        Debug.Log("Is Idling");
    }

    private void FireVolleyAttack()
    {
        Debug.Log("Is Attacking");
    }
}
