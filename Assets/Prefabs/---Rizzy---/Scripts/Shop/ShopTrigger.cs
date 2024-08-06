using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public ShopItem item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IInteractable interactable = item.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
