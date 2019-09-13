using System.Collections;
using UnityEngine;

public class BallKiller : MonoBehaviour
{
    [SerializeField] private float _killDelay;
    [SerializeField] private Lifebar _lifebar;
    [SerializeField] private BallCatcher _catcherForRespawn;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Ball ball = collider.GetComponentInParent<Ball>();
        if(ball != null)
        {
            Rigidbody2D ballBody = collider.GetComponentInParent<Rigidbody2D>();
            
            if(ballBody != null)
            {
                ballBody.simulated = false;
                ballBody.isKinematic = true;
                ball.ResetSpeed();
            }

            StartCoroutine(DestroyBallCoroutine(ball));
        }
    }

    private IEnumerator DestroyBallCoroutine(Ball ball)
    {
        Time.timeScale = .3f;
        _lifebar.RemoveLife();
        yield return new WaitForSeconds(_killDelay * Time.timeScale);
        Time.timeScale = 1f;
        _catcherForRespawn.CatchBall(ball);
    }
}
