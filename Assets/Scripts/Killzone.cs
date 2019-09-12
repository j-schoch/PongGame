using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // destroy its entire family hierarchy
        Destroy(collider.transform.root.gameObject);
    }
}
