using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    private float currentHealth; // Current health of the player

    void Start()
    {
        currentHealth = maxHealth; // Initialize the player's health to maximum at the start
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
        // Add logic for player death, such as playing a death animation or restarting the level
        Destroy(gameObject);
    }

    // Optional method to heal the player
    public void Heal(float amount)
    {
        currentHealth += amount; // Increase the player's health by the healing amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health does not exceed maxHealth
        Debug.Log("Player healed: " + amount + ", Current health: " + currentHealth);
    }
}
