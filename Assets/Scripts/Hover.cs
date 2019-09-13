using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour
{
    [SerializeField] private float _hoverDistance;
    [SerializeField] private float _hoverLoopDuration;

    private void Start()
    {
        transform.DOMoveY(0.5f, 1f).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
