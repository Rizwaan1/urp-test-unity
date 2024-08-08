using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using System.Collections;
using System;
using Demo.Scripts.Runtime.Character;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float lowHealthThreshold = 20f; // Health threshold to trigger low health feedback
    public float regenerationRate = 5f; // Health points regained per second
    public float regenerationDelay = 5f; // Seconds before regeneration starts
    public MMFeedbacks getHitFeedBack, onDeathFeedBack, onLowHealthFeedBack; // Feedbacks
    public GameObject objectToSpawn; // Object to spawn on death
    public Transform spawnPoint; // Spawn point for the object
    public Text healthText; // Reference to the UI Text component
    public Slider healthSlider; // Reference to the UI Slider component

    public float walkSpeed = 5f; // Walk speed of the player
    public float runSpeed = 10f; // Run speed of the player

    private float currentHealth; // Current health of the player
    private Rigidbody rb; // Rigidbody component reference
    private bool lowHealthFeedbackPlayed = false; // To ensure the low health feedback is played only once
    private Coroutine regenerationCoroutine; // Reference to the regeneration coroutine

    private FPSMovement fpsMovement; // Reference to the FPSMovement script
    private PurchaseManager purchaseManager; // Reference to the PurchaseManager script

    public static event Action OnPlayerDamaged;

    public float CurrentHealth => currentHealth; // Public getter for current health
    public float MaxHealth => maxHealth; // Public getter for max health

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        fpsMovement = GetComponent<FPSMovement>();
        if (fpsMovement != null)
        {
            fpsMovement.AdjustMovementSpeed(walkSpeed, runSpeed);
        }

        purchaseManager = FindObjectOfType<PurchaseManager>();
        if (purchaseManager == null)
        {
            Debug.LogError("PurchaseManager not found in the scene.");
        }

        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage: " + amount + ", Current health: " + currentHealth);
        getHitFeedBack?.PlayFeedbacks();
        UpdateHealthUI();

        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
        }

        if (currentHealth <= lowHealthThreshold && !lowHealthFeedbackPlayed)
        {
            onLowHealthFeedBack?.PlayFeedbacks();
            lowHealthFeedbackPlayed = true;
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            regenerationCoroutine = StartCoroutine(RegenerateHealth());
        }

        OnPlayerDamaged?.Invoke();
    }

    void Die()
    {
        Debug.Log("Player died!");
        onDeathFeedBack?.PlayFeedbacks();

        if (objectToSpawn != null && spawnPoint != null)
        {
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        }

        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log("Player healed: " + amount + ", Current health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth > lowHealthThreshold)
        {
            lowHealthFeedbackPlayed = false;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenerationDelay);

        while (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            Debug.Log("Regenerating health: " + currentHealth);

            UpdateHealthUI();

            yield return null;
        }
    }

    public void ApplyDamagezombie(float damage)
    {
        TakeDamage(damage);
    }

    public void PickupHealthObject(float healAmount)
    {
        Heal(healAmount);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth.ToString("F0");
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        Debug.Log("Max health increased by " + amount + ". New max health: " + maxHealth);

        UpdateHealthUI();
    }

    public void IncreaseMovementSpeed(float walkAmount, float runAmount)
    {
        walkSpeed += walkAmount;
        runSpeed += runAmount;
        if (fpsMovement != null)
        {
            fpsMovement.AdjustMovementSpeed(walkSpeed, runSpeed);
        }
        Debug.Log("Movement speed increased. New walk speed: " + walkSpeed + ", New run speed: " + runSpeed);
    }

    public void PurchaseAndApplyItem(ShopItem item, float cost)
    {
        if (purchaseManager != null && purchaseManager.PurchaseItem(cost))
        {
            item.ApplyItem();
            Debug.Log("Purchased and applied item: " + item.itemType);
        }
        else
        {
            Debug.Log("Not enough money to purchase the item.");
        }
    }
}
