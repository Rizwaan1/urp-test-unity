using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    private float currentHealth; // Current health of the player
    private Rigidbody rb; // Rigidbody component reference
    private bool isDead = false; // Flag to check if the player is dead
    public string deadLayerName = "DeadPlayer"; // Name of the layer to switch to when dead

    public PlayerController controller;

    void Start()
    {
        currentHealth = maxHealth; // Initialize the player's health to maximum at the start
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        if (rb != null)
        {
            rb.freezeRotation = true; // Optionally freeze rotation to prevent unwanted rotation
        }
    }

    // Method to handle taking damage
    public void TakeDamage(float amount)
    {
        if (isDead) return; // If player is dead, do nothing

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

        isDead = true; // Mark the player as dead
        
        // Change the player's layer to "DeadPlayer"
        gameObject.layer = LayerMask.NameToLayer(deadLayerName);


        // Disable the Rigidbody's rotation constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None; // Remove all constraints to allow free movement
            rb.freezeRotation = false; // Allow rotation
            rb.AddTorque(Vector3.up * 500f); // Add torque to make the player fall over
           
        }

        // Start coroutine to re-enable constraints after 2 seconds
        StartCoroutine(ReenableConstraintsAfterDelay(2f));

        // Add logic for player death, such as playing a death animation or restarting the level
        //Destroy(gameObject, 4f); // Destroy the player object after 4 seconds
    }

    // Coroutine to re-enable Rigidbody constraints after a delay
    IEnumerator ReenableConstraintsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Re-enable Rigidbody constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; // Freeze X-Z position and all rotations

            // freeze player movement 
            controller.Stasis = true;
        }
    }

    // Optional method to heal the player
    public void Heal(float amount)
    {
        if (isDead) return; // If player is dead, do nothing

        currentHealth += amount; // Increase the player's health by the healing amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health does not exceed maxHealth
        Debug.Log("Player healed: " + amount + ", Current health: " + currentHealth);
    }
}
