using UnityEngine;

public class BulletPerfect : MonoBehaviour
{
    public float speed = 20f; // Snelheid van de kogel
    public float lifeTime = 2f; // Levensduur van de kogel voordat deze wordt vernietigd
    public float damage = 10f; // Schade die de kogel toebrengt
    public Rigidbody rb; // Verwijzing naar de Rigidbody-component
    public GameObject bulletVisual, explosionPrefab;
    public Transform bulletTransform;


    public bool forPlayer,forEnemy;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // Schiet de kogel naar voren
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Zorg voor nauwkeurige detectie bij hoge snelheid

        Destroy(gameObject, lifeTime); // Vernietig de kogel na een bepaalde tijd om geheugen vrij te maken
    }

    void OnCollisionEnter(Collision collision)
    {
        // Functie die wordt aangeroepen wanneer de kogel iets raakt
        HandleCollision(collision);

        // Vernietig de kogel na impact
        Destroy(bulletVisual);
        Destroy(gameObject);
        
    }

    void HandleCollision(Collision collision)
    {
        // Instantiate explosie effect
        Instantiate(explosionPrefab, bulletTransform.position, bulletTransform.rotation);

        if (forPlayer)
        {

            // Logica voor het raken van een vijand
            if (collision.gameObject.CompareTag("Player"))
            {
                // Voorbeeld van het toebrengen van schade aan een vijand
                PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage); // Schade toebrengen aan de vijand
                }
            }

            // Andere logica voor andere soorten objecten kan hier ook worden toegevoegd
            Debug.Log("Bullet hit: " + collision.gameObject.name);
        }

        

            // Logica voor het raken van een vijand
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Voorbeeld van het toebrengen van schade aan een vijand
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Schade toebrengen aan de vijand
                }
            }

            // Andere logica voor andere soorten objecten kan hier ook worden toegevoegd
            Debug.Log("Bullet hit: " + collision.gameObject.name);
       
       
    }
}
