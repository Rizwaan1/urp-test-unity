using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour , IInteractable
{

    public AudioSource source;
    public AudioClip ComputerSound;
    // Start is called before the first frame update
    void Start()
    {
        //source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        source.PlayOneShot(ComputerSound);


    }
}
