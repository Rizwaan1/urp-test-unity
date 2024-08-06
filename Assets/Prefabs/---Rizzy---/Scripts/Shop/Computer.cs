using Demo.Scripts.Runtime.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    public AudioSource source;
    public AudioClip ComputerSound, buySound;
    public GameObject uiPanel; // Referentie naar het UI-paneel dat je wilt tonen
    public FPSController fpsController;  // Voeg een referentie toe naar FPSController
    public GameObject weaponPrefab;  // Referentie naar het wapen prefab dat je wilt kopen
    public float weaponCost = 100f;  // Kost van het wapen

    private void Start()
    {
        // Initialiseer indien nodig
    }

    private void Update()
    {
        // Update indien nodig
    }

    public void Interact()
    {
        // Speel het computer geluid af
       

        // Toon het UI-paneel
       // uiPanel.SetActive(true);

        // Koop het wapen
        if (fpsController != null)
        {
            fpsController.BuyWeapon(weaponPrefab, weaponCost);
            Debug.Log("Bought Weapn");
            source.PlayOneShot(buySound);
        }
        else
        {
            Debug.LogError("FPSController reference is not set.");
            source.PlayOneShot(ComputerSound);
        }
    }
}
