
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    public GameObject impactEffect;

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy bullet after a certain time
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Spawn impact effect at the point of collision
        if (impactEffect != null)
        {
            Instantiate(impactEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        }

       
    }
}
