using System.Collections;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip[] enemySounds; // Array van mogelijke geluiden
    public float minTimeBetweenSounds = 5.0f; // Minimale tijd tussen geluiden
    public float maxTimeBetweenSounds = 15.0f; // Maximale tijd tussen geluiden

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component niet gevonden. Voeg een AudioSource component toe aan dit object.");
            return;
        }

        if (enemySounds.Length == 0)
        {
            Debug.LogError("Geen geluiden toegewezen aan enemySounds array.");
            return;
        }

        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            float delay = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
            yield return new WaitForSeconds(delay);

            if (audioSource.isPlaying)
            {
                continue; // Wacht totdat het huidige geluid is gestopt
            }

            AudioClip randomSound = enemySounds[Random.Range(0, enemySounds.Length)];
            audioSource.PlayOneShot(randomSound);
        }
    }
}
