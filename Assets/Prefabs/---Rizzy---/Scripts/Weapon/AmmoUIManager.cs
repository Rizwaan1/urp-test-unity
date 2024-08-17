using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour
{
    public Text currentAmmoText;
    public Text maxAmmoBeltText;

    public void UpdateAmmoUI(int currentAmmo, float maxAmmoBelt)
    {
        if (currentAmmoText != null)
        {
            currentAmmoText.text = "Ammo: " + currentAmmo.ToString();
        }

        if (maxAmmoBeltText != null)
        {
            maxAmmoBeltText.text = "Ammo Belt: " + maxAmmoBelt.ToString();
        }
    }
}
