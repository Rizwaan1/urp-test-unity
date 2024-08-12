using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints; // Voeg hier je spawn points toe

    void Start()
    {
        SpawnZombies(100); // Aantal zombies dat je wilt spawnen
    }

    void SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(zombiePrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
        }
    }
}
