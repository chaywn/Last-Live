using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light light;

    [Header("Second light (light source)")]
    public bool hasSecondLight;
    public Light light2;

    void Start()
    {
        light = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        int count;
        
        while (true) {
            // Flicker every 5 to 10 seconds
            yield return new WaitForSeconds(Random.Range(5, 11));

            // Flicker once or twice everytime it happens
            count = Random.Range(1, 3);

            for (int i=0; i<count; i++) {
                light.enabled = false;
                if (hasSecondLight) light2.enabled = false;

                yield return new WaitForSeconds(0.1f);

                light.enabled = true;
                if (hasSecondLight) light2.enabled = true;

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
