using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Enemy Abilities/Shock Attack Ability")]
public class ShockAttackAbility : EnemyAbility
{
    public GameObject initialShockPrefab;
    public GameObject damageShockPrefab;
    public float shockDelay = 2f;
    public float shockDamage = 20f;
    public List<string> targetTags; // Tags van mogelijke doelwitten (bijv. "Player", "Enemy")
    public List<float> additionalShockDelays = new List<float> { 2f, 4f }; // Voorbeeld vertragingen voor de extra schokken

    public override void ExecuteAbility(GameObject enemy, GameObject target, Transform firePoint = null, GameObject bulletPrefab = null)
    {
        target = FindClosestTarget(enemy.transform);

        if (target != null)
        {
            GameObject initialShock = Instantiate(initialShockPrefab, target.transform.position, Quaternion.identity);
            ShockEffect shockEffect = initialShock.GetComponent<ShockEffect>();
            if (shockEffect != null)
            {
                shockEffect.Initialize(target.transform, shockDelay, shockDamage, damageShockPrefab, additionalShockDelays);
            }
        }
    }

    private GameObject FindClosestTarget(Transform enemyTransform)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (string tag in targetTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distanceToTarget = Vector3.Distance(enemyTransform.position, potentialTarget.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = potentialTarget;
                }
            }
        }

        return closestTarget;
    }
}
