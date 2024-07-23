using UnityEngine;
using UnityEngine.UI; // Voor UI-elementen zoals een health bar

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 100; // Maximale gezondheid van de speler
    private int currentHealth; // Huidige gezondheid van de speler

    public HealthBar healthBar; // Verwijzing naar een health bar UI-element

    void Start()
    {
        currentHealth = maxHealth; // Stel de huidige gezondheid in op het maximum bij de start
        healthBar.SetMaxHealth(maxHealth); // Stel de health bar in op de maximale gezondheid
    }

    // Functie om schade toe te brengen aan de speler
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Verminder de huidige gezondheid met de hoeveelheid schade
        healthBar.SetHealth(currentHealth); // Update de health bar

        if (currentHealth <= 0)
        {
            Die(); // Roep de Die-functie aan als de gezondheid op of onder nul komt
        }
    }

    // Functie om de speler te genezen
    public void Heal(int amount)
    {
        currentHealth += amount; // Verhoog de huidige gezondheid met de hoeveelheid genezing
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Zorg ervoor dat de gezondheid niet boven het maximum komt
        }
        healthBar.SetHealth(currentHealth); // Update de health bar
    }

    // Functie om af te handelen wat er gebeurt als de speler sterft
    void Die()
    {
        Debug.Log("Player Died");
        // Voeg hier de logica toe voor wat er moet gebeuren wanneer de speler sterft,
        // zoals het tonen van een game over-scherm of het opnieuw laden van het level
    }
}
