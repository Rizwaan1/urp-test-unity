using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCUAttack : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject bulletPrefab; // De prefab die je wilt instantiate
    public Transform firePointL; // Het punt van waaruit de prefab wordt geinstantieerd
    public Transform firePointR;
    public float fireRate = 0.5f; // De tijd tussen elke schot in seconden
    private Transform tankTransform;

    public Transform playerTransform;

    public AudioSource source;
    public AudioClip alarmSound;

    private float nextTimeToFire = 0f;

    

    public enum FiringState
    {
        Idle = 1,
        FireVolley,
        FireGrenade
    }

    public FiringState firingState;
    void Start()
    {
        tankTransform = GetComponent<Transform>();
    }

    void Update()
    {
        
        switch (firingState)
        {
            case FiringState.Idle: IdleState(); break;

            case FiringState.FireVolley: FireVolleyAttack(); break;

            case FiringState.FireGrenade: FireVolleyAttack(); break;

            default: firingState = FiringState.Idle; IdleState(); break;
        }     
    }

    private void IdleState()
    {
        bulletsFired = 0;
        waitTimerV = 0;
        Debug.Log("Is Idle" + tankTransform);
    }

    //Timer functionality and Shooting
    [SerializeField] float waitDuration = 2f;
    private float waitTimerV = 0;

    [SerializeField] private int bulletsToFire = 3;
    private int bulletsFired = 0;

    private bool isWaitingHoming = false;
    private bool isFiringHoming = false;

    private void FireVolleyAttack()
    {
        //source.PlayOneShot(alarmSound);
        if (waitTimerV < waitDuration)
        {
            isWaitingHoming = true;
            
            waitTimerV += Time.deltaTime;
        }
           
        else
        {
            //Debug.Log("Start firing");
            isFiringHoming = true;
        }        
        
        if (isFiringHoming)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                source.PlayOneShot(alarmSound);
                bulletsFired += 1;
            }

            if (bulletsFired >= bulletsToFire)
            {
                isFiringHoming = false;
                bulletsFired = 0;
                waitTimerV = 0;
                firingState = FiringState.FireGrenade;
            }
        }
              Debug.Log("Is Attacking" + tankTransform);
    }
    
  
    //Turret vars
    private int currentTurret;

    void Shoot()
    {
        
        if (currentTurret == 0)
        {
            Instantiate(bulletPrefab, firePointL.position, firePointL.rotation);
            currentTurret = 1;
        }
        else
        {
            Instantiate(bulletPrefab, firePointR.position, firePointR.rotation);
            currentTurret = 0;
        }        
    }

    void CheckPlayerSide()
    {
        // Vector from the enemy to the player
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Check if the player is to the right or left of the enemy
        float dotProduct = Vector3.Dot(transform.right, directionToPlayer);

        if (dotProduct > 0)
        {
            //Fire Grenade Right
            Debug.Log("Player is to the right of the enemy.");
        }
        else if (dotProduct < 0)
        {
            //Fire Grenade Left
            Debug.Log("Player is to the left of the enemy.");
        }
    }

    private void FireGrenadeAttack()
    {
        CheckPlayerSide();
        firingState = FiringState.FireVolley;
    }


}
