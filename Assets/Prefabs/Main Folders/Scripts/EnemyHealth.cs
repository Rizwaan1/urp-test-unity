using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 50f;
    public AudioSource source;
    public AudioClip DamageClip;
    public Color damageColor = Color.red;
    public float sizeChangeFactor = 1.2f;
    public float changeDuration = 0.2f;
    public Text healthText; // Referentie naar de UI Text

    private Color originalColor;
    private Vector3 originalScale;
    private Renderer enemyRenderer;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
        originalScale = transform.localScale;

        UpdateHealthUI(); // Zorg ervoor dat de UI tekst correct is bij de start
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        source.PlayOneShot(DamageClip);
        StartCoroutine(DamageEffect());
        UpdateHealthUI(); // Update de UI tekst bij schade

        if (health <= 0f)
        {
            Die();
        }
    }

    private IEnumerator DamageEffect()
    {
        // Kleurverandering
        enemyRenderer.material.color = damageColor;

        // Size verandering
        Vector3 newScale = originalScale * sizeChangeFactor;
        transform.localScale = newScale;

        // Wacht een bepaalde tijd
        yield return new WaitForSeconds(changeDuration);

        // Terug naar oorspronkelijke kleur
        enemyRenderer.material.color = originalColor;

        // Terug naar oorspronkelijke grootte
        transform.localScale = originalScale;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(health).ToString(); // Toon health als geheel getal
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

