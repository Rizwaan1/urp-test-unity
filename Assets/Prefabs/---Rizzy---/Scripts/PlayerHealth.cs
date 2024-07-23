using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    private float currentHealth; // Current health of the player
    private Rigidbody rb; // Rigidbody component reference

    void Start()
    {
        currentHealth = maxHealth; // Initialize the player's health to maximum at the start
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        if (rb != null)
        {
            // Optionally freeze rotation to prevent unwanted rotation
            rb.freezeRotation = true;
        }
    }

    // Method to handle taking damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Reduce the player's health by the damage amount
        Debug.Log("Player took damage: " + amount + ", Current health: " + currentHealth);
        maxHealth = currentHealth;

        // Check if the player's health is depleted
        if (currentHealth <= 0f)
        {
            Die(); // Call the Die method if health is depleted
        }
    }

    // Method to handle player death
    void Die()
    {
        Debug.Log("Player died!");

        // Disable the Rigidbody's rotation constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; // Keep the player on the X-Z plane
            rb.freezeRotation = false; // Allow rotation
            rb.AddTorque(Vector3.up * 500f); // Add torque to make the player fall over
        }

        // Add logic for player death, such as playing a death animation or restarting the level
        //Destroy(gameObject, 2f); // Destroy the player object after 2 seconds
    }

    // Optional method to heal the player
    public void Heal(float amount)
    {
        currentHealth += amount; // Increase the player's health by the healing amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health does not exceed maxHealth
        Debug.Log("Player healed: " + amount + ", Current health: " + currentHealth);
    }
}
