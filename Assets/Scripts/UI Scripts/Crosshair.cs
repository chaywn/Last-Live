using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crosshair : MonoBehaviour
{
    TextMeshProUGUI text;

    Color white = new Color(1, 1, 1, 1);
    Color red = new Color(0.588f, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ColorRed()
    {
        text.color = red;
    }

    public void ColorWhite()
    {
        text.color = white;
    }
}
