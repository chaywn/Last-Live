using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveText : MonoBehaviour
{

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateWave(int wave)
    {   
        text.text = "Wave " + wave;
    }

    public void Intermission()
    {
        text.text = "Intermission...";
    }

}
