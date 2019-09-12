using UnityEngine;

public class InitialAngularVelocity : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private bool _randomStrength;
    [SerializeField] private float _randomStrengthMax;
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().angularVelocity = _randomStrength ? GetRandomStrength() : _strength;
    }

    private float GetRandomStrength()
    {
        return Random.Range(-_randomStrengthMax, _randomStrengthMax);
    }
}
