using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies; // Array van vijanden voor deze golf
    public float spawnInterval; // Tijd tussen het spawnen van vijanden in deze golf
}

public class WaveSystem : MonoBehaviour
{
    public int maxWaves = 10;
    public Transform playerTransform; // Referentie naar de speler
    public Transform[] spawnPoints; // Array van spawn points
    public List<Wave> enemiesPerWave; // Lijst van waves
    public Text waveText;
    public float timeBetweenWaves = 5.0f; // Tijd tussen golven
    public float spawnRadius = 20.0f; // Radius waarin spawn points geldig zijn voor spawnen

    private int currentWave = 0;
    private bool waveActive = false;
    private int zombiesAlive;

    void Start()
    {
        if (enemiesPerWave.Count != maxWaves)
        {
            Debug.LogError("Zorg ervoor dat de lengte van enemiesPerWave overeenkomt met maxWaves.");
            return;
        }
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (waveActive && zombiesAlive <= 0)
        {
            waveActive = false;
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator StartNextWave()
    {
        if (currentWave < maxWaves)
        {
            currentWave++;
            zombiesAlive = enemiesPerWave[currentWave - 1].enemies.Length; // Haal het aantal vijanden voor de huidige golf op
            waveText.text = "Wave: " + currentWave;

            yield return new WaitForSeconds(timeBetweenWaves);

            StartCoroutine(SpawnWave());
        }
        else
        {
            waveText.text = "All waves completed!";
            // Je kunt hier extra code toevoegen voor wat er gebeurt als alle golven zijn voltooid.
        }
    }

    IEnumerator SpawnWave()
    {
        waveActive = true;

        GameObject[] enemiesToSpawn = enemiesPerWave[currentWave - 1].enemies;
        float spawnInterval = enemiesPerWave[currentWave - 1].spawnInterval;

        List<Transform> validSpawnPoints = GetValidSpawnPoints();

        if (validSpawnPoints.Count == 0)
        {
            Debug.LogError("Geen geldige spawn points binnen de spawn radius.");
            yield break;
        }

        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            Transform spawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
            SpawnEnemy(enemiesToSpawn[i], spawnPoint);
            yield return new WaitForSeconds(spawnInterval); // Delay tussen spawns om te voorkomen dat alles in één keer spawnt
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        Vector3 randomSpawnPosition = GetRandomPositionAroundPoint(spawnPoint.position, 5.0f); // 5.0f is de spawn variatie binnen de spawn point
        GameObject enemy = Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);
        Zombie_CS enemyScript = enemy.GetComponent<Zombie_CS>();
        if (enemyScript != null)
        {
            enemyScript.waveSystem = this;
        }
    }

    List<Transform> GetValidSpawnPoints()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPoints)
        {
            float distance = Vector3.Distance(playerTransform.position, spawnPoint.position);
            if (distance <= spawnRadius)
            {
                validSpawnPoints.Add(spawnPoint);
            }
        }

        return validSpawnPoints;
    }

    Vector3 GetRandomPositionAroundPoint(Vector3 point, float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 spawnPosition = new Vector3(point.x + randomPoint.x, point.y, point.z + randomPoint.y);
        return spawnPosition;
    }

    public void ZombieKilled()
    {
        zombiesAlive--;
        Debug.Log("Zombie killed. Remaining: " + zombiesAlive);
    }
}
