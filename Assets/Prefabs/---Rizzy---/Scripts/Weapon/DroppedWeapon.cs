using Demo.Scripts.Runtime.Character;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    public GameObject weaponPrefab;  // Het daadwerkelijke wapen prefab dat aan de speler wordt toegevoegd

    private void OnTriggerEnter(Collider other)
    {
        // Controleer of het de speler is die het wapen oppakt
        if (other.CompareTag("Player"))
        {
            // Probeer de FPSController van de speler te verkrijgen
            FPSController fpsController = other.GetComponent<FPSController>();

            if (fpsController != null)
            {
                // Voeg het wapen toe aan de speler's inventaris
                fpsController.BuyWeapon(weaponPrefab, 0f);  // Je kunt de kosten hier op 0 zetten aangezien het een pickup is

                // Vernietig het gedropte wapen object nadat het is opgepakt
                Destroy(gameObject);
            }
        }
    }
}
