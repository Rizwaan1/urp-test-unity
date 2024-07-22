using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform playerTarget;
    [SerializeField] private float speed = 7.5f;

    void Update()
    {
        transform.LookAt(playerTarget.position);
        transform.Translate(speed * Vector3.forward * Time.deltaTime);
    }
}
