using UnityEngine;
using TMPro;

public class HighscoreDisplayItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Init(ScoreKeeper.Highscore highscore)
    {
        _nameText.text = highscore.name.ToString();
        _scoreText.text = highscore.score.ToString();
    }
}
