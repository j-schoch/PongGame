using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class used to forward collision events to a different object for convenience
/// </summary>
public class ColliderEvents : MonoBehaviour
{
    [Serializable]
    public class UnityEvent_Collider2D : UnityEvent<Collider2D> {}
    
    public UnityEvent_Collider2D OnTriggerEnter = new UnityEvent_Collider2D();

    public void OnTriggerEnter2D(Collider2D collider)
    {
        OnTriggerEnter.Invoke(collider);
    }
}
