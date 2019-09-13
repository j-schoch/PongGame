using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantAngularVelocity : MonoBehaviour
{
    [SerializeField] private float angularVelocity;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.angularVelocity = angularVelocity;
    }
}
