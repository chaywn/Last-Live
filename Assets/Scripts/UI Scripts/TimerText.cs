using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    TextMeshProUGUI text;

    AudioSource audioSource;

    [Tooltip("In seconds")]public int timer;

    public bool timerReached = false;

    Color white = new Color(1, 1, 1, 1);
    Color red = new Color(0.588f, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void RestartTimer(int timeInSeconds, bool intermission = false)
    {
        StopAllCoroutines();
        timer = timeInSeconds;
        timerReached = false;

        if (intermission) {
            text.color = white;
        }
        else {
            text.color = red;
        }

        StartCoroutine(CountdownTimer());
    }

    IEnumerator CountdownTimer()
    {
        while (timer>0 && !timerReached) {
            
            if (timer < 6) {
                SwitchColor();
                Invoke(nameof(SwitchColor), 0.5f);
                audioSource.PlayOneShot(audioSource.clip);
            }   

            yield return new WaitForSeconds(1);

            timer --;
            UpdateTimer();
        }
        timerReached = true; 
    }

    void UpdateTimer()
    {
        int minute = timer/60;
        int second = timer%60;

        text.text = minute.ToString("00") + ":" + second.ToString("00");
    }

    void SwitchColor()
    {
        text.color = text.color == red ? white : red;
    }

    public void Visibility(bool visible)
    {
        text.enabled = visible;
    }
}
