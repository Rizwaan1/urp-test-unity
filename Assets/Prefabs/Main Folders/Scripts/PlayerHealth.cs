using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Maximaal aantal gezondheidspunten
    public int maxHealth = 100;
    // Huidige gezondheid van de speler
    private int currentHealth;

    // Start wordt een keer aangeroepen voor de eerste update
    void Start()
    {
        // Zet de huidige gezondheid op het maximale gezondheid bij het begin
        currentHealth = maxHealth;
    }

    // Methode om schade aan te brengen
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage: " + damage);

        // Controleer of de gezondheid op of onder nul is
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Methode om de speler te genezen
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player healed: " + amount);
    }

    // Methode die wordt aangeroepen als de speler sterft
    void Die()
    {
        Debug.Log("Player died!");
        // Voeg hier logica toe voor wat er moet gebeuren als de speler sterft
        // Bijvoorbeeld: herstarten van het niveau, tonen van een doodsscherm, etc.
    }

    // Optioneel: Methode om de huidige gezondheid op te vragen
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
