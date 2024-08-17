using UnityEngine;

public class BulletVisuals : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public float damage = 10f;

    void Start()
    {
        // Vernietig de kogel na een bepaalde tijd om geheugen te besparen
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Laat de kogel vooruit bewegen
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Logica voor het raken van een vijand
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Vernietig de kogel bij botsing
        Destroy(gameObject);
    }
}
