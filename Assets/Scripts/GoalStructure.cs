using UnityEngine;

public class GoalStructure : MonoBehaviour
{
    [SerializeField] private PaddleController _owner;
    [SerializeField] private GameObject _aliveDisplay;
    [SerializeField] private GameObject _deadDisplay;

    private void Awake()
    {
        _owner.GoalAdded();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Ball ball = collider.GetComponentInParent<Ball>();
        if(ball != null)
        {
            _aliveDisplay.SetActive(false);
            _deadDisplay.SetActive(true);
            _owner.GoalDestroyed();
        }
    }
}
