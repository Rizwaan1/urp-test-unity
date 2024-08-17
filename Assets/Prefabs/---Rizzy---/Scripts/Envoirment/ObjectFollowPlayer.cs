using UnityEngine;

public class ObjectFollowPlayer : MonoBehaviour
{
    public Transform player; // Referentie naar de speler
    public float followRadius = 5.0f; // Radius waarin het object de speler moet volgen
    public float moveSpeed = 2.0f; // Snelheid waarmee het object beweegt

    void Update()
    {
        // Bereken de afstand tussen het object en de speler
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Controleer of de speler binnen de radius is
        if (distanceToPlayer <= followRadius)
        {
            // Beweeg het object richting de speler
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // Optionele debug functie om de radius in de Scene View te zien
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
