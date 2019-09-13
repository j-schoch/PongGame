using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _initialSpeed;
    [SerializeField] private float _maxSpeed;

    private float _speedMultiplier;
    private float _constantSpeed;
    private float _totalSpeed;
    private Rigidbody2D _rigidbody;

    public PaddleController LastPaddleHit
    {
        get; private set;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void StartGame()
    {
        _speedMultiplier = 1f;
        _constantSpeed = _initialSpeed;
        _rigidbody.velocity = Random.value > 0.5 ? Vector3.up : Vector3.down;
    }

    private void Update()
    {
        if(transform.position.magnitude > 100)
        {
            transform.position = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        _totalSpeed = Mathf.Min(_constantSpeed * _speedMultiplier, _maxSpeed);
        Vector3 newVelocity = _rigidbody.velocity.normalized * _totalSpeed;
        _rigidbody.velocity = Vector3.ClampMagnitude(newVelocity, _maxSpeed);
    }

    public void ChangeSpeedAndSetDirection(float changeInSpeed, Vector2 newDirection)
    {
        ChangeSpeed(changeInSpeed);
        float realSpeed = _rigidbody.velocity.magnitude;
        if(realSpeed == 0)
        {
            // if the velocity is zero, give it the direction vector so the direction is applied and valid
            _rigidbody.velocity = newDirection;
        }
        else
        {
            ChangeDirection(newDirection);
        }
    }

    public void ChangeSpeed(float changeInSpeed)
    {
        _constantSpeed += changeInSpeed;
    }

    /// <summary>
    /// Fails if the ball isn't currently moving
    /// </summary>
    /// <param name="newDirection"></param>
    public void ChangeDirection(Vector2 newDirection)
    {
        _rigidbody.velocity = newDirection * _rigidbody.velocity.magnitude;
    }

    public void ChangeSpeedMultiplier(float changeInMultiplier)
    {
        _speedMultiplier += changeInMultiplier;
    }

    public void ResetSpeedMultiplier()
    {
        _speedMultiplier = 1f;
    }

    public void ResetSpeed()
    {
        _constantSpeed = 0f;
        ResetSpeedMultiplier();
    }

    public void HitByPaddle(PaddleController paddle)
    {
        LastPaddleHit = paddle;
    }
}
