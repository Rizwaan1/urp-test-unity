using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    public AudioSource source;
    public AudioClip ComputerSound;
    public GameObject uiPanel; // Reference to the UI Panel that you want to show
    private PlayerController playerController; // Reference to the player's script

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object and get the PlayerController component
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        // Play the computer sound
        source.PlayOneShot(ComputerSound);


        // Show the UI Panel
        uiPanel.SetActive(true);


    }
}
