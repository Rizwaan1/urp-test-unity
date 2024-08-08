using UnityEngine;
using MoreMountains.Feedbacks;
using Unity.Mathematics;

public class BulletPerfect : MonoBehaviour
{
    public float speed = 20f; // Snelheid van de kogel
    public float lifeTime = 2f; // Levensduur van de kogel voordat deze wordt vernietigd
    public float damage = 10f; // Schade die de kogel toebrengt
    public Rigidbody rb; // Verwijzing naar de Rigidbody-component
    public GameObject bulletVisual, explosionPrefab;
    public Transform bulletTransform;

    public KeyCode NoiseKey;
    [HideInInspector]
    public bool Noise;
    float timer;

    [SerializeField] public MMFeedbacks onHit, onSpawn;

    public bool forPlayer, forEnemy;

    private const string PlayerTag = "Player";
    private const string EnemyTag = "Enemy";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // Schiet de kogel naar voren
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Zorg voor nauwkeurige detectie bij hoge snelheid
        onSpawn?.PlayFeedbacks();

        Noise = true;
        timer = 0.1f;

        Destroy(gameObject, lifeTime); // Vernietig de kogel na een bepaalde tijd om geheugen vrij te maken
    }

    private void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }

        if (Noise && timer <= 0.0f)
        {
            Noise = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
        Destroy(bulletVisual);
        Destroy(gameObject);
    }

    void HandleCollision(Collision collision)
    {
        if (forPlayer && collision.gameObject.CompareTag(PlayerTag))
        {
            ApplyDamageToPlayer(collision);
        }

        if (forEnemy)
        {
            if (collision.gameObject.CompareTag(EnemyTag))
            {
                ApplyDamageToEnemy(collision);
            }
            else
            {
                ApplyDamageToZombie(collision);
            }
        }

        onHit?.PlayFeedbacks();
        Debug.Log("Bullet hit: " + collision.gameObject.name);
    }

    void ApplyDamageToPlayer(Collision collision)
    {
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    void ApplyDamageToEnemy(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    void ApplyDamageToZombie(Collision collision)
    {
        Zombie_CS zombie = collision.gameObject.GetComponent<Zombie_CS>();
        if (zombie != null)
        {
            zombie.EnemyDamage(damage);
        }
    }
}
