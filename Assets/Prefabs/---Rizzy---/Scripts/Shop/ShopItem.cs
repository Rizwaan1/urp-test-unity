using UnityEngine;

public class ShopItem : MonoBehaviour, IInteractable
{
    public enum ItemType
    {
        MaxHealth,
        MovementSpeed
    }

    public ItemType itemType;
    public float value; // The value to increase (e.g., health amount or speed)
    public float cost; // The cost of the item

    private PlayerHealth playerHealth;
    private PurchaseManager purchaseManager;
    private bool isPurchased = false; // Track if the item has been purchased

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
            isPurchased = true; // Mark the item as purchased
        }
        else
        {
            Debug.Log("Not enough money to purchase the item.");
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
                playerHealth.IncreaseMovementSpeed(value, value * 2); // Example: walkAmount and runAmount
                break;
        }
        Debug.Log("Purchased and applied item: " + itemType);
    }
}
