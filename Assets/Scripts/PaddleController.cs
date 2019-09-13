using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PaddleController : MonoBehaviour
{
    [Header("Key Bindings")]
    [SerializeField] private KeyCode _moveLeftKey;
    [SerializeField] private KeyCode _moveRightKey;
    [SerializeField] private KeyCode _catchBallKey;
    [SerializeField] private KeyCode _launchBallKey;

    [Space]
    [SerializeField] private float _moveSpeed;

    [Header("Catch and Launch")]
    [SerializeField] private BallCatcher _ballCatcher;
    [SerializeField] private float _aimSpeed;

    [Space]
    [SerializeField] private float _timeoutCatchAfterBounce;

    [Space]
    [SerializeField] private SpriteRenderer _paddleSprite;

    public bool HoldingBall => _ballCatcher != null && _ballCatcher.HoldingBall;

    public float CurrentMoveSpeed => _moveSpeed * _moveSpeedMultiplier;

    private Rigidbody2D _rigidbody;
    private PlayerInput _currentInputs;
    private Vector2 _directionalInput;
    private float _moveSpeedMultiplier = 1f;

    private bool _aiming;

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    public void Update()
    {
        UpdatePlayerInputs();
        UpdateBallCatch();
    }

    private void UpdatePlayerInputs()
    {
        bool leftJustPressed = Input.GetKeyDown(_moveLeftKey);
        bool rightJustPressed = Input.GetKeyDown(_moveRightKey);        

        // override direction on keypress
        if(leftJustPressed || rightJustPressed)
        {
            if(leftJustPressed) { _directionalInput.x = -1; }
            if(rightJustPressed) { _directionalInput.x = 1; }
        }
        else
        {
            // otherwise set direction based on which is held
            bool leftHeld = Input.GetKey(_moveLeftKey);
            bool rightHeld = Input.GetKey(_moveRightKey);   

            // nothing held
            if(!leftHeld && !rightHeld) { _directionalInput.x = 0; }

            // one direction held
            if(leftHeld && !rightHeld) { _directionalInput.x = -1; }
            if(!leftHeld && rightHeld) { _directionalInput.x = 1; }

            // if both are held, do nothing & whichever was pressed more recently will stay set
        }
        
        // assign new input state
        _currentInputs = new PlayerInput
        {
            catchKeyIsPressed = Input.GetKey(_catchBallKey),
            launchKeyIsPressed = Input.GetKey(_launchBallKey),
            directionalInput = _directionalInput
        };
    }

    private void UpdateBallCatch()
    {
        if(_ballCatcher == null || !_ballCatcher.enabled) return;

        if(HoldingBall)
        {
            if(_currentInputs.launchKeyIsPressed)
            {
                _aiming = true;
                _ballCatcher.SetLaunchVisualizerActive(true);
                _ballCatcher.OffsetLaunchByAngle(_currentInputs.directionalInput.x * _aimSpeed * Time.deltaTime);
            }

            if(_aiming && !_currentInputs.launchKeyIsPressed)
            {
                _aiming = false;
                _ballCatcher.SetLaunchVisualizerActive(false);
                _ballCatcher.LaunchBall();
            }
        }
        else
        {
            if(_currentInputs.catchKeyIsPressed)
            {  
                _ballCatcher.TryCatchBall();
            } 
        }
    }

    public void FixedUpdate()
    {
        if(!_aiming)
        {
            Vector3 velocity = _currentInputs.directionalInput * CurrentMoveSpeed * Time.deltaTime;
            _rigidbody.MovePosition(transform.position + velocity);
        }
    }

    public void AddSpeedMultiplier(float addToMultiplier)
    {
        _moveSpeedMultiplier += addToMultiplier;
    }

    public void ResetSpeedMultiplier()
    {
        _moveSpeedMultiplier = 1;
    }

    public void TimeoutCatch()
    {
        _ballCatcher.enabled = false;
        StartCoroutine(Timer(_timeoutCatchAfterBounce, () => _ballCatcher.enabled = true));
    }

    public void ChangePaddleWidth(float changeInWidth)
    {
        DOTween.Kill(_paddleSprite.transform, complete: true);
        _paddleSprite.transform.DOScaleX(changeInWidth, .5f).SetRelative().SetEase(Ease.OutBack);
    }

    public void ChangePaddleColor(Color color)
    {
        _paddleSprite.DOColor(color, 0.5f);
    }

    private IEnumerator Timer(float time, Action onTimerEnd)
    {
        yield return new WaitForSeconds(time);
        onTimerEnd?.Invoke();
    }

    private struct PlayerInput
    {
        public bool catchKeyIsPressed;
        public bool launchKeyIsPressed;
        public Vector3 directionalInput;
    }
}
