using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryPanel; // Reference to the Inventory UI Panel's RectTransform
    public GameObject gameplayUI; // Reference to the Gameplay UI GameObject
    public Text gameplayMoneyText; // Reference to the Gameplay UI's money Text
    public Text inventoryMoneyText; // Reference to the Inventory UI's money Text
    public float animationDuration = 0.5f; // Duration of the animation
    public AudioSource audioSource; // Single AudioSource to play the sounds
    public AudioClip uiOpenClip; // AudioClip for the sound when UI is opened
    public AudioClip uiCloseClip; // AudioClip for the sound when UI is closed
    public MoneyManager moneyManager; // Reference to the MoneyManager script
    public PlayerInput playerInput; // Reference to the PlayerInput component

    public InputAction openInventoryAction; // Define an InputAction for opening inventory
    public static bool IsInventoryOpen { get; private set; } = false; // Tracks if the inventory is open

    private Vector3 hiddenScale = Vector3.zero;
    private Vector3 visibleScale = Vector3.one;
    private bool isInventoryVisible = false;

    private void Start()
    {
        // Set the initial scale to hidden (0)
        inventoryPanel.localScale = hiddenScale;
        inventoryPanel.gameObject.SetActive(false); // Hide the panel initially

        // Bind the action and enable it
        openInventoryAction.performed += OnOpenInventory;
        openInventoryAction.Enable(); // Ensure the action is enabled
    }

    private void OnDestroy()
    {
        // Unbind the action when the object is destroyed
        openInventoryAction.performed -= OnOpenInventory;
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Toggled Inventory");

        if (isInventoryVisible)
        {
            StartCoroutine(AnimatePanel(hiddenScale, false)); // Animate to hide the panel
            if (audioSource != null && uiCloseClip != null)
            {
                audioSource.clip = uiCloseClip;
                audioSource.Play();
            }

            // Switch back to the gameplay money text
            moneyManager.SetActiveMoneyText(gameplayMoneyText);
            IsInventoryOpen = false; // Allow player to move and shoot
            playerInput.enabled = true; // Re-enable PlayerInput when closing the inventory

            // Hide the cursor and lock it in the center of the screen
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            gameplayUI.SetActive(false); // Disable Gameplay UI
            inventoryPanel.gameObject.SetActive(true); // Show the Inventory panel

            // Play the open sound when the UI is opened
            if (audioSource != null && uiOpenClip != null)
            {
                audioSource.clip = uiOpenClip;
                audioSource.Play();
            }

            // Switch to the inventory money text
            moneyManager.SetActiveMoneyText(inventoryMoneyText);

            StartCoroutine(AnimatePanel(visibleScale, true)); // Animate to show the panel
            IsInventoryOpen = true; // Prevent player from moving and shooting
            playerInput.enabled = false; // Disable PlayerInput when the inventory is open

            // Show the cursor and unlock it
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        isInventoryVisible = !isInventoryVisible;
    }

    private IEnumerator AnimatePanel(Vector3 targetScale, bool activating)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = inventoryPanel.localScale;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            inventoryPanel.localScale = Vector3.Lerp(startingScale, targetScale, t);
            yield return null;
        }

        // Ensure the final scale is exactly the target scale
        inventoryPanel.localScale = targetScale;

        // Disable the Inventory panel and re-enable the Gameplay UI if we are hiding it
        if (!activating)
        {
            inventoryPanel.gameObject.SetActive(false);
            gameplayUI.SetActive(true); // Re-enable Gameplay UI
        }
    }

    private void OnEnable()
    {
        Debug.Log("InventoryManager enabled.");
        openInventoryAction?.Enable(); // Ensure the action is enabled
    }

    private void OnDisable()
    {
        openInventoryAction?.Disable(); // Disable the action when the script is disabled
    }
}
