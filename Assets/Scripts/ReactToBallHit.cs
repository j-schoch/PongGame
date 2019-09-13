using UnityEngine;
using UnityEngine.Events;

public class ReactToBallHit : MonoBehaviour
{
    [SerializeField] private UnityEvent OnHitByBall = new UnityEvent();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.collider.GetComponentInParent<Ball>();
        if(ball != null)
        {
            OnHitByBall.Invoke();
        }
    }
}
