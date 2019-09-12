using UnityEngine;

/// <summary>
/// Sets a collider to be a percentage size of the viewport and centered
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class ViewportColliderStartSize : MonoBehaviour
{
    // use vertical padding in viewport space
    [SerializeField] private float _percentViewportHeightPadding;

    private BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Camera mainCam = Camera.main;
        Vector2 topPoint = new Vector2(0, 1-_percentViewportHeightPadding);
        Vector2 bottomPoint = new Vector2(0, _percentViewportHeightPadding);

        Vector2 worldTop = mainCam.ViewportToWorldPoint(topPoint);
        Vector2 worldBottom = mainCam.ViewportToWorldPoint(bottomPoint);

        _collider.size = new Vector2(_collider.size.x, worldTop.y - worldBottom.y);

        Vector3 newPosition = transform.position;
        newPosition.y = (worldTop.y + worldBottom.y) * 0.5f;
        transform.position = newPosition;
    }
}
