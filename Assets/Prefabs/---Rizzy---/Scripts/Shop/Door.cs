using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Feedbacks;

public class Door : MonoBehaviour, IInteractable
{
    public float cost = 250f;
    private bool isOpen = false;
    private PurchaseManager purchaseManager;
    private Animator animator;
    private NavMeshObstacle navMeshObstacle;
    public MMFeedbacks buyDoor;

    void Start()
    {
        purchaseManager = FindObjectOfType<PurchaseManager>();
        if (purchaseManager == null)
        {
            Debug.LogError("PurchaseManager not found in the scene.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on the door object.");
        }

        navMeshObstacle = GetComponent<NavMeshObstacle>();
        if (navMeshObstacle == null)
        {
            Debug.LogError("NavMeshObstacle not found on the door object.");
        }
        else
        {
            navMeshObstacle.carving = true;
        }
    }

    public void Interact()
    {
        if (!isOpen)
        {
            if (purchaseManager.PurchaseItem(cost))
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Not enough money to open the door.");
            }
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        animator.SetTrigger("Open");
        buyDoor?.PlayFeedbacks();
        navMeshObstacle.enabled = false;
        GetComponent<Collider>().enabled = false;
        Debug.Log("Door is now open!");
    }
}
