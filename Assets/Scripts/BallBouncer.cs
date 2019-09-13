using UnityEngine;

/// <summary>
/// Bounces the ball away from the paddle center
/// </summary>
public class BallBouncer : MonoBehaviour
{
    private ScoreKeeper _scoreKeeper;
    private void Awake()
    {
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.collider.GetComponentInParent<Ball>();
        if(ball != null)
        {
            float speedIncreasePerBounce = 0.1f;

            Vector3 awayFromPaddleDir = (collision.collider.transform.position - transform.position).normalized;
            collision.rigidbody.velocity = awayFromPaddleDir * collision.rigidbody.velocity.magnitude * (1 + speedIncreasePerBounce); // increase ball speed each hit

            var paddle = GetComponentInParent<PaddleController>();
            paddle.AddSpeedMultiplier(speedIncreasePerBounce);
            _scoreKeeper.HitPaddle(paddle);

            //disable catching if the paddle was hit
            paddle.TimeoutCatch();

            ball.HitByPaddle(paddle);
        }
    }
}
