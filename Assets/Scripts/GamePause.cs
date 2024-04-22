using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GamePause : MonoBehaviour
{
    public static bool gameIsPaused;

    public FirstPersonController playerController;

    public AudioSource backgroundMusic;

    [Header("UI")]
    public GameObject gameUI;
    public GameObject pauseUI;
    public TimerText timerText;

    void Start()
    {
        // Allow the background music to continue playing even if the game is paused
        backgroundMusic.ignoreListenerPause = true;

        gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {

            gameIsPaused = !gameIsPaused;

            // Toggle paused boolean in first person controller script to disable RotateView()
            playerController.paused = gameIsPaused;

            // Mute or unmute all audio
            AudioListener.pause = gameIsPaused; 

            // Pause or unpause all time-based actions
            Time.timeScale = gameIsPaused ? 0f : 1f; 

            // Toggle UI
            gameUI.SetActive(!gameIsPaused);
            pauseUI.SetActive(gameIsPaused);
            // 'Hiding' the timer text instead of disabling so that it won't end the coroutine
            timerText.Visibility(!gameIsPaused);
        }
    }
}
