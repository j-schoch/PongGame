using UnityEngine;

public class ExtendPaddlePowerup : Powerup
{
    [SerializeField] private Color _paddleColor;
    [SerializeField] private float _widthExtension;
    protected override void Apply(PaddleController paddle)
    {
        base.Apply(paddle);
        paddle.ChangePaddleWidth(_widthExtension);
        paddle.ChangePaddleColor(_paddleColor);
    }

    protected override void Reset(PaddleController paddle)
    {
        base.Reset(paddle);
        paddle.ChangePaddleWidth(-_widthExtension);
        paddle.ChangePaddleColor(Color.white);
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        Destroy(gameObject);
    }
}
