using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, IInteractable
{
    public AudioSource source;
    public AudioClip ComputerSound;
    public GameObject uiPanel; // Reference to the UI Panel that you want to show

    // Start is called before the first frame update
    void Start()
    {
        // source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        // Play the computer sound
        source.PlayOneShot(ComputerSound);

        // Unlock the mouse cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show the UI Panel
        uiPanel.SetActive(true);
    }
}
