using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    Slider slider;

    public Image bar;

    public Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateHealth(int health)
    {
        slider.value = health;

        bar.color = gradient.Evaluate(slider.normalizedValue);
    }
}
