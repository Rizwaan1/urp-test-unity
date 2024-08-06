using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    private MoneyManager moneyManager;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not found in the scene.");
        }
    }

    public bool PurchaseItem(float cost)
    {
        if (moneyManager != null)
        {
            return moneyManager.SpendMoney(cost);
        }
        return false;
    }
}
