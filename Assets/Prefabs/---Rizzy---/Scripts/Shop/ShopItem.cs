using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour, IInteractable
{
    public enum ItemType
    {
        MaxHealth,
        MovementSpeed
    }

    public ItemType itemType;
    public float value;
    public float cost;

    public AudioSource audioSource;          // Main AudioSource component
    public AudioSource secondaryAudioSource; // Secondary AudioSource for additional clip
    public AudioClip initialClip;            // Clip to play at the beginning (loops)
    public AudioClip purchasedClip;          // Clip to play after purchase (once)
    public AudioClip duringPurchasedClip;    // Clip to play during the purchased clip (once)

    private PlayerHealth playerHealth;
    private PurchaseManager purchaseManager;
    private bool isPurchased = false;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        purchaseManager = FindObjectOfType<PurchaseManager>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not found in the scene.");
        }

        if (purchaseManager == null)
        {
            Debug.LogError("PurchaseManager not found in the scene.");
        }

        // Play the initial audio clip in a loop
        if (audioSource != null && initialClip != null)
        {
            audioSource.clip = initialClip;
            audioSource.loop = true;  // Set the initial clip to loop
            audioSource.Play();
        }
    }

    public void Interact()
    {
        if (isPurchased)
        {
            Debug.Log("This item has already been purchased.");
            return;
        }

        if (purchaseManager.PurchaseItem(cost))
        {
            ApplyItem();
            isPurchased = true;

            // Stop the looping initial clip and play the purchased clip once
            if (audioSource != null && purchasedClip != null)
            {
                audioSource.loop = false;  // Disable looping
                audioSource.clip = purchasedClip;
                audioSource.Play();

                // Play the secondary audio clip during the purchased clip
                if (secondaryAudioSource != null && duringPurchasedClip != null)
                {
                    secondaryAudioSource.clip = duringPurchasedClip;
                    secondaryAudioSource.Play();
                }

                // After the purchased clip finishes, resume the initial clip in a loop
                StartCoroutine(ResumeInitialClipAfterPurchased());
            }
        }
        else
        {
            Debug.Log("Not enough money to purchase the item.");
        }
    }

    private IEnumerator ResumeInitialClipAfterPurchased()
    {
        // Wait for the purchased clip to finish playing
        yield return new WaitWhile(() => audioSource.isPlaying);

        // Resume the initial clip in a loop
        if (audioSource != null && initialClip != null)
        {
            audioSource.clip = initialClip;
            audioSource.loop = true;  // Re-enable looping
            audioSource.Play();
        }
    }

    public void ApplyItem()
    {
        switch (itemType)
        {
            case ItemType.MaxHealth:
                playerHealth.IncreaseMaxHealth(value);
                break;
            case ItemType.MovementSpeed:
                playerHealth.IncreaseMovementSpeed(value, value);
                break;
        }
        Debug.Log("Purchased and applied item: " + itemType);
    }
}
