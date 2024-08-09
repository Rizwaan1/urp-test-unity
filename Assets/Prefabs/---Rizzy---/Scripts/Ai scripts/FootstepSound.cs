using UnityEngine;
using MoreMountains.Feedbacks;

public class FootstepSound : MonoBehaviour
{
    public MMFeedbacks footstepSound;
    

    void Start()
    {
       
    }


    public void PlaySound()
    {
        footstepSound?.PlayFeedbacks();

    }
    
}
