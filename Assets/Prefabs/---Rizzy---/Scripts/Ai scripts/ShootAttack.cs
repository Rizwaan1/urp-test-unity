using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Abilities/Shoot Attack")]
public class ShootAttack : EnemyAbility
{
    public float bulletForce = 20f;
    public float fireRate = 0.1f;
    public int maxBullets = 30;
    public AudioClip shootSound; // Voeg een AudioClip toe voor het schietgeluid

    public override void ExecuteAbility(GameObject enemy, GameObject target, Transform firePoint, GameObject bulletPrefab)
    {
        enemy.GetComponent<MonoBehaviour>().StartCoroutine(ShootCoroutine(enemy, firePoint, bulletPrefab));
    }

    private IEnumerator ShootCoroutine(GameObject enemy, Transform firePoint, GameObject bulletPrefab)
    {
        int bulletsLeft = maxBullets;
        AudioSource audioSource = enemy.GetComponent<AudioSource>(); // Haal de AudioSource op

        while (bulletsLeft > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

            // Speel het schietgeluid af
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            bulletsLeft--;
            yield return new WaitForSeconds(fireRate);
        }
    }
}
