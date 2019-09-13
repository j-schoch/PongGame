using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitialSpriteColor : MonoBehaviour
{
    [SerializeField] private bool _singleColorChildren;
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

        if(_singleColorChildren) 
        {
            int randomIndex = Random.Range(0, colors.Count);
            _spriteRenderers.ForEach(sprite => sprite.color = colors[randomIndex]);
        } 
        else 
        {
            _spriteRenderers.ForEach(sprite => 
            {
                int randomIndex = Random.Range(0, colors.Count);
                sprite.color = colors[randomIndex];
            });
        }
    }
}
