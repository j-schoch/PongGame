using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InitialSpriteColor : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    private List<SpriteRenderer> _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    private void Start()
    {
        if(colors.Count == 0 || _spriteRenderers.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, colors.Count);
        _spriteRenderers.ForEach(sprite => sprite.color = colors[randomIndex]);
    }
}
