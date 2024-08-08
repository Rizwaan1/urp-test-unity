using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Enemy Abilities/Heal Ability")]
public class HealAbility : EnemyAbility
{
    public float healAmount = 50f;
    public float regenerationBoost = 5f;
    public float boostDuration = 10f;
    public AudioClip healSound;

    public override void ExecuteAbility(GameObject enemy, GameObject target, Transform firePoint = null, GameObject bulletPrefab = null)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.CurrentHealth > 0 && playerHealth.CurrentHealth < playerHealth.MaxHealth)
        {
            playerHealth.Heal(healAmount);

            AudioSource audioSource = enemy.GetComponent<AudioSource>();
            if (audioSource != null && healSound != null)
            {
                audioSource.PlayOneShot(healSound);
            }

            enemy.GetComponent<MonoBehaviour>().StartCoroutine(BoostRegeneration(playerHealth));
        }
    }

    private IEnumerator BoostRegeneration(PlayerHealth playerHealth)
    {
        playerHealth.regenerationRate += regenerationBoost;
        yield return new WaitForSeconds(boostDuration);
        playerHealth.regenerationRate -= regenerationBoost;
    }
}
