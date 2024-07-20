using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    // The target object that this object will look at
    public Transform target;

    void Update()
    {
        // Check if the target has been assigned
        if (target != null)
        {
            // Make the object look at the target
            transform.LookAt(target);
        }
    }
}
