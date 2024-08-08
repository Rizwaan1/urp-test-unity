using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Abilities/Spawn Minions Ability")]
public class SpawnMinionsAbility : EnemyAbility
{
    public GameObject minionPrefab; // Prefab van de minion
    public int minionCount = 3; // Aantal minions om te spawnen
    public float spawnRadius = 5f; // Radius rondom de vijand om minions te spawnen

    public override void ExecuteAbility(GameObject enemy, GameObject target, Transform firePoint = null, GameObject bulletPrefab = null)
    {
        for (int i = 0; i < minionCount; i++)
        {
            // Bepaal een willekeurige positie binnen de spawn radius
            Vector3 randomPosition = enemy.transform.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = enemy.transform.position.y; // Zorg ervoor dat de minions op dezelfde hoogte spawnen

            // Spawn de minion
            Instantiate(minionPrefab, randomPosition, Quaternion.identity);
        }
    }
}
