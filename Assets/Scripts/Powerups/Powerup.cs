using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private List<GameObject> _hideOnPickup;

    private void Start()
    {
        transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutBack);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Ball ball = collider.GetComponentInParent<Ball>();
        if(ball != null && ball.LastPaddleHit != null)
        {
            StartCoroutine(ApplyPowerupCoroutine(ball.LastPaddleHit));
        }
    }

    private IEnumerator ApplyPowerupCoroutine(PaddleController paddle)
    {
        Apply(paddle);
        Hide();
        yield return new WaitForSeconds(_duration);
        Reset(paddle);
        Cleanup();
    }

    protected virtual void Apply(PaddleController paddle)
    {
    }

    protected virtual void Reset(PaddleController paddle)
    {
    }

    protected virtual void Cleanup()
    {
    }
    
    protected virtual void Hide()
    {
        foreach(GameObject go in _hideOnPickup)
        {
            go.SetActive(false);
        }
    }
}
