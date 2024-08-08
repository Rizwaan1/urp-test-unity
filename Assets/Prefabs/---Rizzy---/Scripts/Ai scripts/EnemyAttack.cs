using UnityEngine;

public abstract class EnemyAttack : ScriptableObject
{
    public abstract void ExecuteAttack(GameObject enemy, Transform firePoint, GameObject bulletPrefab);
}
