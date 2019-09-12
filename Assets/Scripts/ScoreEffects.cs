using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class ScoreEffects : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _multiplierText;
    [SerializeField] private GameObject _textParticlePrefab;
    [SerializeField] private int _prePooledObjects;
    private List<TextMeshProUGUI> _textObjectsPool = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> _activeTextObjects = new List<TextMeshProUGUI>();

    private void Awake()
    {
        for(int i = 0; i < _prePooledObjects; i++)
        {
            AddTextObject();
        }
    }

    private TextMeshProUGUI AddTextObject()
    {
        TextMeshProUGUI newText = Instantiate(_textParticlePrefab,Vector3.zero, Quaternion.identity, transform).GetComponent<TextMeshProUGUI>();
        newText.gameObject.SetActive(false);
        _textObjectsPool.Add(newText);
        return newText;
    }

    public void MultiplierChanged()
    {
        DOTween.Kill(_multiplierText.transform, true);
        _multiplierText.transform.DOPunchScale(Vector2.one * .2f, .3f).SetRelative();
    }

    public void ScoreChanged(int totalChange, Transform relevantLocation = null)
    {
        TextMeshProUGUI text = GetNextUnusedTextObject();

        if(relevantLocation != null)
        {
            // move text away from location in the local upward direction
            Vector3 textOffset = relevantLocation.up * 2;
            text.transform.position = relevantLocation.position + textOffset;
        }
        else
        {
            text.transform.position = transform.position;
        }

        text.transform.localScale = Vector3.one;
        text.text = $"+{totalChange}";

        text.transform.DOScale(Vector3.zero, .5f)
            .From()
            .SetEase(Ease.OutBack)
            .OnComplete(() => 
            {
                text.transform.DORotate(new Vector3(0,0,360), .5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.InBack);
                text.transform.DOScale(Vector3.zero, .5f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() => {
                        text.gameObject.SetActive(false);
                    });
            });

        text.gameObject.SetActive(true);
    }

    private TextMeshProUGUI GetNextUnusedTextObject()
    {
        TextMeshProUGUI text = _textObjectsPool.Find(t => !t.gameObject.activeSelf);
        if(text == null)
        {
            text = AddTextObject();
        }
        return text;
    }
}
