using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{

    public static Player instance;

    public GameManager gameManager;

    public AudioClip[] audioClipArray;

    AudioSource audiosource0;
    AudioSource audiosource1;

    [Header("Blur Effect")]
    public PostProcessVolume PostProcessVolume;
    DepthOfField dof;
    bool blurring = false;
    public float blurDuration;

    [Header("Health")]
    public int health = 100;
    public HealthBar healthBar;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PostProcessVolume.profile.TryGetSettings(out dof);

        audiosource0 = GetComponent<AudioSource>();

        // Create a new audiosource for heartbeat sounds
        audiosource1 = gameObject.AddComponent<AudioSource>();
        audiosource1.clip = audioClipArray[1];
        audiosource1.loop = true;
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        // Decrease health by damage
        health -= damage;

        // Refresh health bar
        healthBar.UpdateHealth(health);

        // Play the hurt sound (adjusting volume to make it louder)
        audiosource0.volume = 1.0f;
        audiosource0.PlayOneShot(audioClipArray[0]);
        audiosource0.volume = 0.75f;

        if (health <= 35) {
            // Start playing the heartbeat sound if the player is low on health
            audiosource1.Play();

            // Start the blur effect on the player's screen
            if (!blurring) {
                blurring = true;
                StartCoroutine(Blur());
            }

            // Transition to gameover scene if the player is dead
            if (health <= 0) {
                gameManager.GameOver();
            }
        }
    }

    IEnumerator Blur()
    {
        float t = 0;
        float startingValue = dof.focalLength.value; // 1
        float endValue = 2.75f; 

        // Go from 1 to 2.75 (no blur to blur) in the first Lerp
        // After that, go back and forth between 2.75 and 4 (less blur and more blur) to create a smooth blur effect
        while (true) {
            while (t < blurDuration) {
                dof.focalLength.value = Mathf.Lerp(startingValue, endValue, t);
                t += Time.deltaTime/blurDuration;

                yield return null;
            }
            dof.focalLength.value = endValue;

            // reset the interpolation timer and switch the startingValue and endValue
            t = 0;
            startingValue = startingValue == 2.75f ? 4 : 2.75f;
            endValue = startingValue == 2.75f ? 4 : 2.75f;
        }
    }

}
