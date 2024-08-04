using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour
{
    public int maxWaves = 10;
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;
    public Text waveText;
    public float timeBetweenWaves = 5.0f;

    private int currentWave = 0;
    private bool waveActive = false;
    private int zombiesPerWave;
    private int zombiesAlive;

    void Start()
    {
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
            zombiesPerWave = currentWave * 2; // Increase the number of zombies per wave
            waveText.text = "Wave: " + currentWave;

            yield return new WaitForSeconds(timeBetweenWaves);

            StartCoroutine(SpawnWave());
        }
        else
        {
            waveText.text = "All waves completed!";
            // You can add any additional code here for what happens when all waves are completed.
        }
    }

    IEnumerator SpawnWave()
    {
        waveActive = true;
        zombiesAlive = zombiesPerWave;

        for (int i = 0; i < zombiesPerWave; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(0.5f); // Delay between spawns to avoid all spawning at once
        }
    }

    void SpawnZombie()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    public void ZombieKilled()
    {
        zombiesAlive--;
    }
}
