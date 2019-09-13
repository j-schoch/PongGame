using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

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

    [SerializeField] private GameObject _lightningPaddle;
    [SerializeField] private GameObject _powerPaddle;
    [SerializeField] private GameObject _swiftPaddle;

    public bool HoldingBall => _ballCatcher != null && _ballCatcher.HoldingBall;
    public float CurrentMoveSpeed => _moveSpeed * _moveSpeedMultiplier;

    public int GoalsRemaining
    {
        get; private set;
    }

    private Rigidbody2D _rigidbody;
    private PlayerInput _currentInputs;
    private Vector2 _directionalInput;
    private float _moveSpeedMultiplier = 1f;
    private bool _aiming;
    private ScoreKeeper _scoreKeeper;
    public float SpeedIncreasePerBounce = 0.1f;

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void Update()
    {
        UpdatePlayerInputs();
        UpdateBallCatch();
    }

    public void SelectLightingPaddle()
    {
        _lightningPaddle.SetActive(true);
        _powerPaddle.SetActive(false);
        _swiftPaddle.SetActive(false);
        _paddleSprite.enabled=false;
        _paddleSprite.transform.localScale = new Vector3(6,1,1);
        _ballCatcher.transform.localScale = new Vector3(1.5f,1,1);
    }

    public void SelectPowerPaddle()
    {
        _lightningPaddle.SetActive(false);
        _powerPaddle.SetActive(true);
        _swiftPaddle.SetActive(false);
        _paddleSprite.enabled=false;

        SpeedIncreasePerBounce = .4f;
    }

    public void SelectSwiftPaddle()
    {
        _lightningPaddle.SetActive(false);
        _powerPaddle.SetActive(false);
        _swiftPaddle.SetActive(true);
        _paddleSprite.enabled=false;

        _moveSpeedMultiplier = 2.25f;
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
        _paddleSprite.DOBlendableColor(color, 0.5f);
    }

    public void GoalAdded()
    {
        GoalsRemaining++;
    }

    public void GoalDestroyed()
    {
        GoalsRemaining--;
        if(GoalsRemaining <= 0)
        {
            _scoreKeeper.EndGame();
        }
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
