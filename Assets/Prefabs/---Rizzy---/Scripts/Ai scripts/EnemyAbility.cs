using UnityEngine;

public abstract class EnemyAbility : ScriptableObject
{
    public abstract void ExecuteAbility(GameObject enemy, GameObject target, Transform firePoint = null, GameObject bulletPrefab = null);
}
