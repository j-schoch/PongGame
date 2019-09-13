using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    private Rigidbody2D _rigidbody;

    public PaddleController LastPaddleHit
    {
        get; private set;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
    }

    public void HitByPaddle(PaddleController paddle)
    {
        LastPaddleHit = paddle;
    }
}
