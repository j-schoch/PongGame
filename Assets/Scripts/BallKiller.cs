using System.Collections;
using UnityEngine;

public class BallKiller : MonoBehaviour
{
    [SerializeField] private float _killDelay;
    [SerializeField] private Lifebar _lifebar;
    [SerializeField] private BallCatcher _catcherForRespawn;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponentInParent<Ball>() != null)
        {
            Rigidbody2D ballBody = collider.GetComponentInParent<Rigidbody2D>();
            
            if(ballBody != null)
            {
                ballBody.simulated = false;
                ballBody.isKinematic = true;
                ballBody.velocity = Vector2.zero;
            }

            StartCoroutine(DestroyBallCoroutine(collider.gameObject));
        }
    }

    private IEnumerator DestroyBallCoroutine(GameObject ball)
    {
        Time.timeScale = .5f;
        _lifebar.RemoveLife();
        yield return new WaitForSeconds(_killDelay);
        Time.timeScale = 1f;
        _catcherForRespawn.CatchBall(ball);
    }
}
