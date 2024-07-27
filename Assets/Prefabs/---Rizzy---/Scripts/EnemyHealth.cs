using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;
    public AudioSource source;
    public AudioClip DamageClip, deathClip;
    public Text healthText; // Reference to the UI Text
    public GameObject spawnOnDeath; // Reference to the GameObject to spawn on death

    private Vector3 originalScale;
    private Renderer enemyRenderer;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;

        UpdateHealthUI(); // Ensure the UI text is correct at the start
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        source.PlayOneShot(DamageClip);
        UpdateHealthUI(); // Update the UI text on damage

        if (health <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(health).ToString(); // Show health as an integer
        }
    }

    void Die()
    {
        source.PlayOneShot(deathClip);
        if (spawnOnDeath != null)
        {
            Instantiate(spawnOnDeath, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
