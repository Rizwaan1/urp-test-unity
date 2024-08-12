using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public Transform target; // Het doel, bijvoorbeeld de speler
    public float speed = 5.0f; // Vliegsnelheid
    public float flightHeight = 10.0f; // Standaard vlieghoogte
    public float minHeight = 2.0f; // Minimale hoogte waarop de vijand mag komen
    public float shootingRange = 15.0f; // De afstand waarop de vijand kan schieten
    public float shootCooldown = 2.0f; // Tijd tussen schoten
    public GameObject bulletPrefab; // De prefab van de kogel
    public Transform bulletSpawnPoint; // De positie waar de kogel wordt gespawnd
    public float sideMovementDistance = 2.0f; // Afstand die de vijand willekeurig naar links of rechts kan bewegen

    private float shootTimer;

    void Update()
    {
        // Bereken de richting naar het doel
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);

        // Zorg ervoor dat de hoogte van de vijand nooit onder de minimale hoogte komt
        targetPosition.y = Mathf.Max(targetPosition.y, minHeight);

        // Als het doel hoger is dan de standaard vlieghoogte, vlieg dan omhoog
        if (targetPosition.y > flightHeight)
        {
            targetPosition.y = flightHeight;
        }

        // Beweeg de vijand in de richting van het doel
        Vector3 direction = targetPosition - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Optioneel: draai de vijand om in de vliegrichting te kijken
        transform.LookAt(targetPosition);

        // Controleer of de vijand binnen het bereik is om te schieten
        if (direction.magnitude <= shootingRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = shootCooldown;
            }
        }
    }

    void Shoot()
    {
        // Willekeurig naar links of rechts bewegen
        Vector3 sideMovement = Vector3.zero;
        int randomDirection = Random.Range(0, 3); // 0 = geen beweging, 1 = links, 2 = rechts
        if (randomDirection == 1)
        {
            sideMovement = transform.right * -sideMovementDistance; // Beweeg naar links
        }
        else if (randomDirection == 2)
        {
            sideMovement = transform.right * sideMovementDistance; // Beweeg naar rechts
        }

        transform.position += sideMovement;

        // Instantiate de kogel en geef deze een richting
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}
