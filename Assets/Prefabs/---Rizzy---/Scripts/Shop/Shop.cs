using UnityEngine;

public class Shop : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public void PurchaseItem(ShopItem item, float cost)
    {
        playerHealth.PurchaseAndApplyItem(item, cost);
    }
}
