using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavesSurvivedText : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        
        DisplayWavesSurvived();
    }

    void DisplayWavesSurvived()
    {
        text.text = "Wave(s) Survived: " + GameManager.wavesSurvived;
    }
}
