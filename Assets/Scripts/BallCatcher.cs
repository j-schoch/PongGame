using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    [SerializeField] private LayerMask _ballLayers;
    [SerializeField] private Transform _holdPosition;
    [SerializeField] private Transform _launchDirectionVisualizer;

    [SerializeField] private float _launchStrength;
    [SerializeField] private float _launchDirectionAngleMax;

    public bool HoldingBall => _heldBall != null;

    private Rigidbody2D _heldBall;
    private Collider2D _catchCollider;
    private float _launchAngleOffset;
    private float _timeHeld;
    private ScoreKeeper _scoreKeeper;

    private void Awake()
    {
        _catchCollider = GetComponent<Collider2D>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public bool TryCatchBall()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_ballLayers);

        // get the first ball overlapping
        Collider2D[] results = new Collider2D[1];
        if(_catchCollider.OverlapCollider(contactFilter, results) > 0)
        {
            CatchBall(results[0].gameObject);
            return true;
        }

        return false;
    }

    public void LaunchBall()
    {
        if(HoldingBall)
        {
            _heldBall.simulated = true;
            _heldBall.isKinematic = false;

            _heldBall.transform.SetParent(null);

            _heldBall.velocity =  GetOffsetLaunchDirection() * _launchStrength;
            _heldBall.GetComponentInChildren<TrailRenderer>().emitting = true;

            _heldBall = null;

            _launchAngleOffset = 0;
            _timeHeld = 0;
            _launchDirectionVisualizer.localRotation = Quaternion.identity;
        }
    }

    public void SetLaunchVisualizerActive(bool active)
    {
        _launchDirectionVisualizer.gameObject.SetActive(active);
    }

    public void OffsetLaunchByAngle(float angle)
    {
        _launchAngleOffset += angle;
        _launchAngleOffset = Mathf.Clamp(_launchAngleOffset, -_launchDirectionAngleMax, _launchDirectionAngleMax);

        _launchDirectionVisualizer.up = GetOffsetLaunchDirection();
    }

    public Vector3 GetOffsetLaunchDirection()
    {
        return Quaternion.Euler(0, 0, _launchAngleOffset) * transform.up;
    }
    
    public void CatchBall(GameObject ball)
    {
        _heldBall = ball.GetComponentInParent<Rigidbody2D>();

        if(HoldingBall)
        {
            _heldBall.GetComponentInChildren<TrailRenderer>().emitting = false;
            _heldBall.velocity = Vector2.zero;
            _heldBall.simulated = false;
            _heldBall.isKinematic = true;

            _heldBall.transform.position = _holdPosition.position;
            _heldBall.transform.SetParent(_holdPosition);
            var paddle = GetComponentInParent<PaddleController>();
            paddle.ResetSpeedMultiplier();
            _scoreKeeper.CaughtBall(paddle);
        }
    }
}
