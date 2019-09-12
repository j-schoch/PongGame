using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InitialVelocity : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private bool _randomStrength;
    [SerializeField] private float _randomStrengthMax;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private bool _randomDirection;
    private void Start()
    {
        float strength = _randomStrength ? GetRandomStrength() : _strength;
        Vector2 direction = (_randomDirection ? RandomPointOnCircle(1) : _direction).normalized;
        GetComponent<Rigidbody2D>().velocity =  direction * strength;
    }

    private Vector2 RandomPointOnCircle(float radius)
    {
        float angle = Random.Range (0f, Mathf.PI * 2);
        float x = Mathf.Sin (angle) * radius;
        float y = Mathf.Cos (angle) * radius;

        return new Vector2 (x, y);
    }

    private float GetRandomStrength()
    {
        return Random.Range(0, _randomStrengthMax);
    }
}
