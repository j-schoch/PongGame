using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ScoreKeeper : MonoBehaviour
{
    private static string _highscore_key = "Highscore";

    [Serializable] public class Highscore 
    {
        public string name = "None";
        public int score = 0;
    }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _multiplierText;
    [SerializeField] private ScoreEffects _scoreEffects;
    [SerializeField] private int _highscoreListLength = 5;
    [SerializeField] private HighscoreUI _highscoreUI;
    [SerializeField] private EndOfGameUI _endOfGameUI;
    [SerializeField] private GameOverUI _gameOverUI;

    [SerializeField] private PaddleController _player1;
    [SerializeField] private PaddleController _player2;
    [SerializeField] private Ball _ball;

    private List<Highscore> _highscores = new List<Highscore>();

    private int _scoreMultiplier = 1;
    private int _score;
    private PaddleController _lastPaddleHit;

    private void Awake()
    {
        LoadHighscores();
        ResetMultiplier();
        ResetScore();
    }

    private void LoadHighscores()
    {
        for(int i = 0; i < _highscoreListLength; i++)
        {
            string key = _highscore_key + i;

            var hs = new Highscore();
            if(PlayerPrefs.HasKey(key))
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), hs);
            }
            else
            {
                PlayerPrefs.SetString(key, JsonUtility.ToJson(hs));
            }

            _highscores.Add(hs);
        }

        PlayerPrefs.Save();
    }

    public void AddScoreMultiplier(int multiplierChange)
    {
        SetScoreMultiplier(_scoreMultiplier + multiplierChange);
    }

    public void MultiplyScoreMultiplier(int multiplier)
    {
        SetScoreMultiplier(_scoreMultiplier * multiplier);
    }

    public void ResetMultiplier()
    {
        SetScoreMultiplier(1);
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    public void AddScore(int scoreChange, Transform relevantLocation = null)
    { 
        int totalChange = scoreChange * _scoreMultiplier;
        if(totalChange > 0)
        {
            _scoreEffects.ScoreChanged(totalChange, relevantLocation);
        }
        SetScore(_score + totalChange);
    }


    // probably could be somewhere more relevant but this class will keep track of volleys adding points for each successive hit
    public void HitPaddle(PaddleController paddle)
    {
        if(_lastPaddleHit == null)
        {
            _lastPaddleHit = paddle;
            return;
        }

        if(_lastPaddleHit != paddle)
        {
            AddScore(1, paddle.transform);
            AddScoreMultiplier(1);
            _lastPaddleHit = paddle;
        }
    }

    public void CaughtBall(PaddleController paddle)
    {
        _lastPaddleHit = paddle;
        ResetMultiplier();
    }

    public void SetScore(int newScore)
    {
        if(newScore != _score)
        {
            _score = newScore;
        }
        _scoreText.text = $"Score: {_score}";
    }

    public void SetScoreMultiplier(int newMultiplier)
    {
        if(newMultiplier != _scoreMultiplier)
        {
            _scoreMultiplier = newMultiplier;
            _multiplierText.text = $"x{_scoreMultiplier}";
            _scoreEffects.MultiplierChanged();
        }
    }

    public void EndGame()
    {
        _ball.ResetSpeed();
        _gameOverUI.Show(_player1, _player2, () => 
        {
            if(HasNewHighscore())
            {
                _highscoreUI.Show(_score, OnNewHighscoreSaved);
            }
            else
            {
                _endOfGameUI.Show(_highscores);
            }
        });
    }

    public bool HasNewHighscore()
    {
        for(int i = 0; i < _highscoreListLength; i++)
        {
            if(_highscores[i].score < _score)
            {
                return true;
            }
        }
        return false;
    }

    private void OnNewHighscoreSaved(Highscore highscore)
    {
        // add the new highscore
        _highscores.Add(highscore);

        // sort them by score
        _highscores = _highscores.OrderByDescending(hs => hs.score).ToList();

        // take only the top section
        _highscores = _highscores.GetRange(0, _highscoreListLength);

        // save them out
        SaveHighscores();

        _endOfGameUI.Show(_highscores);
    }

    private void SaveHighscores()
    {
        for(int i = 0; i < _highscoreListLength; i++)
        {
            string key = _highscore_key + i;
            PlayerPrefs.SetString(key, JsonUtility.ToJson(_highscores[i]));
        }

        PlayerPrefs.Save();
    }
}
