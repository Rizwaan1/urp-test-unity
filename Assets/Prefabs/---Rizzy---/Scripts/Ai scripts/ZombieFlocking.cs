using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFlocking : MonoBehaviour
{
    public float neighborRadius = 10.0f;
    public float separationWeight = 1.5f;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float maxSpeed = 3.0f;
    public float rotationSpeed = 5.0f; // Nieuw toegevoegd voor rotatie

    private Vector3 flockingVelocity;

    void Update()
    {
        List<Transform> neighbors = GetNeighbors();

        if (neighbors.Count == 0)
        {
            Debug.Log("No neighbors found");
            return;
        }

        Vector3 separation = Separation(neighbors) * separationWeight;
        Vector3 alignment = Alignment(neighbors) * alignmentWeight;
        Vector3 cohesion = Cohesion(neighbors) * cohesionWeight;

        Vector3 flockingMove = separation + alignment + cohesion;
        flockingMove = flockingMove.normalized * maxSpeed;

        // Rotatie in de bewegingsrichting
        if (flockingMove != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flockingMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Verplaatsing van de zombie
        transform.position += flockingMove * Time.deltaTime;
    }

    List<Transform> GetNeighbors()
    {
        List<Transform> neighbors = new List<Transform>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighborRadius);
        foreach (Collider collider in colliders)
        {
            if (collider != this.GetComponent<Collider>())
            {
                neighbors.Add(collider.transform);
            }
        }
        return neighbors;
    }

    Vector3 Separation(List<Transform> neighbors)
    {
        Vector3 separationVector = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            separationVector += transform.position - neighbor.position;
        }
        return separationVector.normalized;
    }

    Vector3 Alignment(List<Transform> neighbors)
    {
        Vector3 averageDirection = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            averageDirection += neighbor.forward;
        }
        return averageDirection.normalized;
    }

    Vector3 Cohesion(List<Transform> neighbors)
    {
        Vector3 centerOfMass = Vector3.zero;
        foreach (Transform neighbor in neighbors)
        {
            centerOfMass += neighbor.position;
        }
        centerOfMass /= neighbors.Count;
        return (centerOfMass - transform.position).normalized;
    }
}
