using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    public UnityColliderEvent onTriggerEnter;
    public UnityCollisionEvent onCollisionEnter;
    [System.Serializable]
    public class UnityCollisionEvent : UnityEvent<Collision> { }
    [System.Serializable]
    public class UnityColliderEvent : UnityEvent<Collider> { }

    private void OnTriggerEnter(Collider other)
    {
            onTriggerEnter?.Invoke(other);
    }
    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(collision);
    }
}
