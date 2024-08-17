using Demo.Scripts.Runtime.Character; // Voeg de juiste namespace toe
using Demo.Scripts.Runtime.Item;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public float refillAmount = 30f; // Amount of ammo to refill

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            // Refill the ammo belt and destroy the pickup object
            weapon.RefillAmmoBelt(refillAmount);
            Destroy(gameObject);
        }
    }
}
