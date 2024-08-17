using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Verander de cursor
    public Texture2D cursorTexture; // Sleep hier je aangepaste cursor afbeelding in de editor
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        // Stel het aangepaste cursoricoon in
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    // Deze functie wordt aangeroepen wanneer je op de "Start Game"-knop klikt
    public void StartGame()
    {
        // Laad de eerste scène (meestal het spel zelf)
        SceneManager.LoadScene("GameScene"); // Vervang "GameScene" door de naam van jouw scène
    }

    // Deze functie wordt aangeroepen wanneer je op de "Options"-knop klikt
    public void OpenOptions()
    {
        // Open het opties menu of laad een andere scène met opties
        Debug.Log("Open Options Menu");
        // Hier zou je een ander canvas of scène voor de opties kunnen laden
    }

    // Deze functie wordt aangeroepen wanneer je op de "Quit"-knop klikt
    public void QuitGame()
    {
        // Sluit het spel af
        Debug.Log("Quit Game");
        Application.Quit();
    }

    void OnDisable()
    {
        // Reset de cursor naar de standaardcursor wanneer het script wordt uitgeschakeld of het object wordt vernietigd
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
