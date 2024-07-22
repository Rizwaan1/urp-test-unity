using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCUAttack : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject bulletPrefab; // De prefab die je wilt instantiate
    public Transform firePoint; // Het punt van waaruit de prefab wordt geinstantieerd
    public float fireRate = 0.5f; // De tijd tussen elke schot in seconden
    private Transform tankTransform;

    private float nextTimeToFire = 0f;

    

    public enum FiringState
    {
        Idle = 1,
        FireVolley
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

            default:
                firingState = FiringState.Idle; IdleState(); break;
        }

      
    }

    private void IdleState()
    {
        Debug.Log("Is Idle" + tankTransform);
    }

    private void FireVolleyAttack()
    {
       
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        

        Debug.Log("Is Attacking" + tankTransform);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
