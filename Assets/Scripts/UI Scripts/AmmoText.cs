using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateAmmo(bool reloading, int currentAmmo, int magSize)
    {
        if (reloading) {
            text.text = "Reloading...";
        }
        else {
            text.text = currentAmmo + "/" + magSize;
        }
    }
}
