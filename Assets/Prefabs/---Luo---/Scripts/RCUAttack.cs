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

    public bool canShoot;

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
            case FiringState.Idle: break;

            case FiringState.FireVolley: break;

            default:
                firingState = FiringState.Idle; break;
        }

        if (canShoot)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    private void IdleState()
    {
        Debug.Log("Is Idle" + tankTransform);
    }

    private void FireVolleyAttack()
    {
        
        Debug.Log("Is Attacking" + tankTransform);
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
