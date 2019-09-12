using UnityEngine;

/// <summary>
/// Sets the object position on start to a 2D position on the viewport.
/// </summary>
public class ViewportStartPosition : MonoBehaviour
{
    [SerializeField] private Vector2 _viewportPosition;
    private void Start()
    {
        Vector3 viewportStart = Camera.main.ViewportToWorldPoint(_viewportPosition);
        Vector3 startPosition = transform.position;
        startPosition.x = viewportStart.x;
        startPosition.y = viewportStart.y;
        transform.position = startPosition;
    }
}
