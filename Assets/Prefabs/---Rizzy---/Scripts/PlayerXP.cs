using UnityEngine;
using UnityEngine.UI;

public class PlayerXP : MonoBehaviour
{
    public int currentXP;  // De huidige hoeveelheid XP die de speler heeft
    public int currentLevel;  // Het huidige level van de speler
    public int xpToNextLevel;  // Hoeveel XP nodig is om naar het volgende level te gaan

    // UI referenties
    public Text currentXPText;
    public Text currentLevelText;
    public Text xpToNextLevelText;

    void Start()
    {
        // Initialiseer de waarden
        currentXP = 0;
        currentLevel = 1;
        xpToNextLevel = CalculateXPToNextLevel(currentLevel);

        // Update de UI bij de start
        UpdateUI();
    }

    void Update()
    {
        // Dit is slechts een test om XP toe te voegen als de speler op de "L" toets drukt
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainXP(50);
        }
    }

    public void GainXP(int amount)
    {
        currentXP += amount;

        // Check of de speler voldoende XP heeft om te levelen
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        // Update de UI
        UpdateUI();
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;  // Resterende XP overzetten naar het nieuwe level
        xpToNextLevel = CalculateXPToNextLevel(currentLevel);  // Bereken hoeveel XP nodig is voor het volgende level

        // Update de UI
        UpdateUI();

        // Optioneel: Voeg hier effecten toe zoals het tonen van een level-up bericht, spelerstats verbeteren, etc.
        Debug.Log("Level Up! Nieuwe level: " + currentLevel);
    }

    int CalculateXPToNextLevel(int level)
    {
        // Gebruik een eenvoudige formule om te berekenen hoeveel XP nodig is voor het volgende level
        return level * 100;
    }

    void UpdateUI()
    {
        // Update de UI teksten
        currentXPText.text = "XP: " + currentXP;
        currentLevelText.text = "Level: " + currentLevel;
        xpToNextLevelText.text = "XP to Next Level: " + xpToNextLevel;
    }
}
