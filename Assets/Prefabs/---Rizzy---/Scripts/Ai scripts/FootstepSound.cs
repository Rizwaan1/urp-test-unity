using UnityEngine;
using MoreMountains.Feedbacks;

public class FootstepSound : MonoBehaviour
{
    public MMFeedbacks footstepSound;
    public Collider[] feetColliders;
    private bool[] feetOnGround;

    void Start()
    {
        feetOnGround = new bool[feetColliders.Length];
        for (int i = 0; i < feetOnGround.Length; i++)
        {
            feetOnGround[i] = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < feetColliders.Length; i++)
        {
            if (!feetOnGround[i] && feetColliders[i] == other && other.CompareTag("Ground"))
            {
                footstepSound?.PlayFeedbacks();
                feetOnGround[i] = true;
                break; // Play feedback only once per contact
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < feetColliders.Length; i++)
        {
            if (feetOnGround[i] && feetColliders[i] == other && other.CompareTag("Ground"))
            {
                feetOnGround[i] = false;
                break; // Reset only the foot that left the ground
            }
        }
    }
}
