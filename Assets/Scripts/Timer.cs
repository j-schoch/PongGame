using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = decimal.Round((decimal)Time.timeSinceLevelLoad, 2).ToString();
    }
}
