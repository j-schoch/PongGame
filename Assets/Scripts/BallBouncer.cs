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

            Vector3 awayFromPaddleDir = (collision.collider.transform.position - transform.position).normalized;
            

            var paddle = GetComponentInParent<PaddleController>();
            _scoreKeeper.HitPaddle(paddle);

            // increase speed per bounce, also add the current paddle horizontal speed
            ball.ChangeSpeedMultiplier(paddle.SpeedIncreasePerBounce);
            ball.ChangeSpeedAndSetDirection(collision.otherRigidbody.velocity.x, awayFromPaddleDir);
            //disable catching if the paddle was hit
            paddle.TimeoutCatch();

            ball.HitByPaddle(paddle);
        }
    }
}
