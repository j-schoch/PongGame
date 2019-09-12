using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _saveButton;

    private Action<ScoreKeeper.Highscore> _callback;
    private int _score;

    public void Show(int score, Action<ScoreKeeper.Highscore> callback)
    {
        _callback = callback;
        _score = score;
        _scoreText.text = score.ToString();
        gameObject.SetActive(true);
        _saveButton.onClick.AddListener(OnSaved);
    }

    private void OnSaved()
    {
        var highscore = new ScoreKeeper.Highscore();
        highscore.name = _nameInput.text;
        highscore.score = _score;
        gameObject.SetActive(false);
        _callback(highscore);
    }
}
