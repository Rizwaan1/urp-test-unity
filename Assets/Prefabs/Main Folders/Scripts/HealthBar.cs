using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    // Stel de maximale waarde van de health bar in
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    // Stel de huidige waarde van de health bar in
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
