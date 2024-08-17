using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public float PlayerMoney = 1000f;  // Player's starting money
    private Text activeMoneyText;  // Reference to the currently active UI Text component

    private void Start()
    {
        // Initial update for the money display
        UpdateMoneyUI();
    }

    public void SetActiveMoneyText(Text moneyText)
    {
        activeMoneyText = moneyText;
        UpdateMoneyUI();  // Update the UI whenever the active text is changed
    }

    public void AddMoney(float amount)
    {
        PlayerMoney += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(float amount)
    {
        if (PlayerMoney >= amount)
        {
            PlayerMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough money.");
            return false;
        }
    }

    public void UpdateMoneyUI()
    {
        if (activeMoneyText != null)
        {
            activeMoneyText.text = "Money: $" + PlayerMoney.ToString("F2");
        }
    }
}
