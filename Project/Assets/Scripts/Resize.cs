using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resize : MonoBehaviour
{

    public float xScale;
    public float yScale;


    void Start()
    {

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        xScale = Screen.width / 1920f;
        yScale = Screen.height / 1080f;

        foreach (var r in GetComponentsInChildren<RectTransform>())
        {
            r.anchoredPosition *= xScale;
            r.sizeDelta *= xScale;
        }

    }
}

