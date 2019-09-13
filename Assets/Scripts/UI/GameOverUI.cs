using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private float _delayBeforeClose;
    [SerializeField] private float _fadeTime;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private CanvasGroup _fadeInObject;

    public void Show(PaddleController player1, PaddleController player2, Action onClose)
    {
        gameObject.SetActive(true);

        if(player1.GoalsRemaining > 0)
        {
            _winText.text = "Player 1 Wins!";
        }
        else if(player2.GoalsRemaining > 0)
        {
            _winText.text = "Player 2 Wins!";
        }

        _gameOverText.transform.localScale = Vector3.zero;
        _winText.transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_fadeInObject.DOFade(0, _fadeTime).From());
        sequence.Append(_gameOverText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.Append(_gameOverText.transform.DOMoveY(4, 1).SetRelative().SetEase(Ease.OutBack));
        sequence.Append(_winText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.AppendInterval(_delayBeforeClose);
        sequence.Append(_fadeInObject.DOFade(0, _fadeTime));
        sequence.Play().OnComplete(() => 
        {
            onClose?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
