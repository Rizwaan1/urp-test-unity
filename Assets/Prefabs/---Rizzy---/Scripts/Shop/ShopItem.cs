using UnityEngine;

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
                playerHealth.IncreaseMovementSpeed(value, value);
                break;
        }
        Debug.Log("Purchased and applied item: " + itemType);
    }
}
