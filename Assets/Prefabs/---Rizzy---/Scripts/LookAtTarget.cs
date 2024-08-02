using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public string enemyTag = "Zombie"; // The tag to identify enemies
    public float detectionRadius = 10f; // The radius within which to detect enemies

    void Update()
    {
        // Find the closest enemy within the detection radius
        Transform closestEnemy = FindClosestEnemy();

        // If a closest enemy is found, make the object look at the enemy
        if (closestEnemy != null)
        {
            transform.LookAt(closestEnemy);
        }
    }

    // Method to find the closest enemy within the detection radius
    Transform FindClosestEnemy()
    {
        // Find all colliders within the detection radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestEnemy = null;
        float closestDistance = detectionRadius;

        // Loop through all colliders to find the closest enemy
        foreach (Collider collider in hitColliders)
        {
            // Check if the collider has the specified enemy tag
            if (collider.CompareTag(enemyTag))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                // Update the closest enemy if this one is closer
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
        }

        return closestEnemy;
    }

    // Optional: Draw the detection radius in the scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
