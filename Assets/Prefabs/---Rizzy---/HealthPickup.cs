using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f; // Amount of health this pickup restores

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Heal the player and destroy the pickup object
            playerHealth.PickupHealthObject(healAmount);
            Destroy(gameObject);
        }
    }
}
