using UnityEngine;

public class DamageShockEffect : MonoBehaviour
{
    private float shockDamage;

    public void Initialize(float damage)
    {
        shockDamage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(shockDamage);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(shockDamage);
            }
        }

        Destroy(gameObject); // Destroy the damage shock effect after applying damage
    }
}
