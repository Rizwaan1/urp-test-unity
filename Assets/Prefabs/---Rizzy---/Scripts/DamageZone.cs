using System.Collections;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f; // Hoeveelheid schade die per seconde wordt toegebracht
    [SerializeField]
    private float damageInterval = 1f; // Interval tussen elke schade (in seconden)

    private bool isPlayerInZone = false; // Flag om bij te houden of de speler in de zone is
    private Coroutine damageCoroutine; // Referentie naar de schade coroutine

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            damageCoroutine = StartCoroutine(ApplyDamage(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    private IEnumerator ApplyDamage(Collider player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        while (isPlayerInZone && playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
