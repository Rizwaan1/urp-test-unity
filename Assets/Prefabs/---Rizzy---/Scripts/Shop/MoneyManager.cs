using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public float PlayerMoney = 1000f;  // Beginbedrag van de speler
    public Text moneyText;  // UI-tekst om het geld weer te geven

    private void Start()
    {
        UpdateMoneyUI();
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

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + PlayerMoney.ToString("F2");
        }
    }
}
