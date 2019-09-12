using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfGameUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject _highscoreDisplayItemPrefab;
    [SerializeField] private Transform _highscoresParent;
    public void Show(List<ScoreKeeper.Highscore> highscores)
    {
        gameObject.SetActive(true);

        for(int i = 0 ; i < highscores.Count; i++)
        {
            GameObject newItemObject = Instantiate(_highscoreDisplayItemPrefab, Vector3.zero, Quaternion.identity, _highscoresParent);
            HighscoreDisplayItem item = newItemObject.GetComponent<HighscoreDisplayItem>();
            item.Init(highscores[i]);
        }

        // restart scene on click
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
