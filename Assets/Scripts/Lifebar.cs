using UnityEngine;
public class Lifebar : MonoBehaviour
{
    [SerializeField] private int _startingLives;
    [SerializeField] private GameObject _lifePrefab;

    private ScoreKeeper _scoreKeeper;

    private void Awake()
    {
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start()
    {
        for(int i = 0; i < _startingLives; i++)
        {
            AddLife();
        }
    }

    public void AddLife()
    {
        GameObject newLife = Instantiate(_lifePrefab);
        newLife.transform.SetParent(transform);
        newLife.transform.localScale = Vector3.one;
    }

    public void RemoveLife()
    {
        if(transform.childCount > 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        else
        {
            _scoreKeeper.EndGame();
        }
    }
}
