using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockEffect : MonoBehaviour
{
    private Transform initialPosition;
    private float shockDelay;
    private float shockDamage;
    private GameObject damageShockPrefab;
    private List<float> additionalShockDelays;
    private bool isActive = true;

    public void Initialize(Transform initialPosition, float shockDelay, float shockDamage, GameObject damageShockPrefab, List<float> additionalShockDelays)
    {
        this.initialPosition = initialPosition;
        this.shockDelay = shockDelay;
        this.shockDamage = shockDamage;
        this.damageShockPrefab = damageShockPrefab;
        this.additionalShockDelays = additionalShockDelays;

        StartCoroutine(ShockSequence());
    }

    private IEnumerator ShockSequence()
    {
        // Initial shock delay
        yield return new WaitForSeconds(shockDelay);

        if (isActive)
        {
            InstantiateDamageShock();
        }

        // Additional shock delays
        foreach (float delay in additionalShockDelays)
        {
            yield return new WaitForSeconds(delay);
            if (isActive)
            {
                InstantiateDamageShock();
            }
        }

        Destroy(gameObject); // Destroy the initial shock effect after the sequence is done
    }

    private void InstantiateDamageShock()
    {
        if (isActive)
        {
            GameObject damageShock = Instantiate(damageShockPrefab, initialPosition.position, Quaternion.identity);
            StartCoroutine(DealDamage(damageShock));
        }
    }

    private IEnumerator DealDamage(GameObject damageShock)
    {
        yield return new WaitForSeconds(0.1f); // Short delay before dealing damage to allow player to move out

        Collider[] colliders = Physics.OverlapSphere(damageShock.transform.position, 2f); // Assuming 2 units is the radius within which the shock deals damage
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(shockDamage);
                }
            }
            else if (collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(shockDamage);
                }
            }
        }

        Destroy(damageShock); // Destroy the damage shock effect after dealing damage
    }

    public void Deactivate()
    {
        isActive = false;
    }
}
