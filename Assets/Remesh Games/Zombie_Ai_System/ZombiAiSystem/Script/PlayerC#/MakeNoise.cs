using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MakeNoise : MonoBehaviour
{

   public KeyCode NoiseKey;
    public MMFeedbacks noiseSoundFeedback;
   public bool Noise;
   float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f) 
        {
            timer -= Time.deltaTime;
        
        }
     
        
        if (Input.GetKeyDown(NoiseKey)) 
        {
            Noise = true;
            timer = 0.1f;
            noiseSoundFeedback?.PlayFeedbacks();
        }
        
        if(Noise == true && timer <= 0.0f) 
        {
        
            Noise = false;
        }
        
    }






}
