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
        if(collision.collider.GetComponentInParent<Ball>() != null)
        {
            Vector3 awayFromPaddleDir = (collision.collider.transform.position - transform.position).normalized;
            collision.rigidbody.velocity = awayFromPaddleDir * collision.rigidbody.velocity.magnitude * 1.02f; // increase ball speed each hit

            var paddle = GetComponentInParent<PaddleController>();
            paddle.AddSpeedMultiplier(.05f);
            _scoreKeeper.HitPaddle(paddle);
        }
    }
}
