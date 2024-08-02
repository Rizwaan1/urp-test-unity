using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SimpleAnimationEventTriggers : MonoBehaviour
{
    public MMFeedbacks onAttack, onWalk, onRunning;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void onAttackhit()
    {
        onAttack?.PlayFeedbacks();


    }

    void footStep()
    {
        onWalk?.PlayFeedbacks();
    }

    void onRun()
    {
        onRunning?.PlayFeedbacks();
    }
}
