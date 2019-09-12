using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject _backgroundSpritePrefab;
    [SerializeField] private float _scrollSpeed;

    public int LoopCount { get; private set; }

    private float _imageWidth;
    private float _distanceSinceLastLoop;

    private void Start()
    {
        int imagesNeeded = 2 - transform.childCount;
        if(imagesNeeded <= 0)
        {
            return;
        }

        for(int i = 0; i < imagesNeeded; i++)
        {
            AddImage();
        }
    }

    public void AddImage()
    {
        GameObject newImage = Instantiate(_backgroundSpritePrefab.gameObject);
        newImage.transform.SetParent(transform);
        
        SpriteRenderer newSpriteRenderer = newImage.GetComponent<SpriteRenderer>();
        _imageWidth = newSpriteRenderer.bounds.size.x;
        newImage.transform.position = transform.position + (Vector3.right * _imageWidth * newImage.transform.GetSiblingIndex());
    }

    private void Update()
    {
        float step = _scrollSpeed * Time.deltaTime;
        transform.Translate(Vector3.left * step);
        _distanceSinceLastLoop += step;

        if(_distanceSinceLastLoop >= _imageWidth)
        {
            Loop();
        }
    }

    private void Loop()
    {
        transform.position += Vector3.right * _imageWidth;
        _distanceSinceLastLoop = 0;
        LoopCount++;
    }
}
