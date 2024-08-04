using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float lowHealthThreshold = 20f; // Health threshold to trigger low health feedback
    public float regenerationRate = 5f; // Health points regained per second
    public float regenerationDelay = 5f; // Seconds before regeneration starts
    public MMFeedbacks getHitFeedBack, onDeathFeedBack, onLowHealthFeedBack; // Feedbacks
    public GameObject objectToSpawn; // Object to spawn on death
    public Transform spawnPoint; // Spawn point for the object

    private float currentHealth; // Current health of the player
    private Rigidbody rb; // Rigidbody component reference
    private bool lowHealthFeedbackPlayed = false; // To ensure the low health feedback is played only once
    private Coroutine regenerationCoroutine; // Reference to the regeneration coroutine

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
        getHitFeedBack?.PlayFeedbacks();

        // Stop the regeneration coroutine if it's running
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
        }

        // Check if the player's health falls below the low health threshold
        if (currentHealth <= lowHealthThreshold && !lowHealthFeedbackPlayed)
        {
            onLowHealthFeedBack?.PlayFeedbacks();
            lowHealthFeedbackPlayed = true; // Ensure the feedback is played only once
        }

        // Check if the player's health is depleted
        if (currentHealth <= 0f)
        {
            Die(); // Call the Die method if health is depleted
        }
        else
        {
            // Start the regeneration coroutine
            regenerationCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    // Method to handle player death
    void Die()
    {
        Debug.Log("Player died!");
        onDeathFeedBack?.PlayFeedbacks();

        // Spawn the object at the specified spawn point
        if (objectToSpawn != null && spawnPoint != null)
        {
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        }

        Destroy(gameObject);
    }

    // Optional method to heal the player
    public void Heal(float amount)
    {
        currentHealth += amount; // Increase the player's health by the healing amount
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // Ensure health does not exceed maxHealth
        Debug.Log("Player healed: " + amount + ", Current health: " + currentHealth);

        // Reset the low health feedback if the player's health goes above the threshold
        if (currentHealth > lowHealthThreshold)
        {
            lowHealthFeedbackPlayed = false;
        }
    }

    // Coroutine to handle health regeneration
    private IEnumerator RegenerateHealth()
    {
        // Wait for the specified delay before starting regeneration
        yield return new WaitForSeconds(regenerationDelay);

        // Regenerate health over time until the player reaches max health or takes damage
        while (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            Debug.Log("Regenerating health: " + currentHealth);
            yield return null;
        }
    }

    // This method will call TakeDamage to reduce the player's health
    public void ApplyDamagezombie(float damage)
    {
        TakeDamage(damage);
    }

    // Method to handle object pickup and heal the player
    public void PickupHealthObject(float healAmount)
    {
        Heal(healAmount);
    }
}
