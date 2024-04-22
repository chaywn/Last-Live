using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int zombieKilled;
    public static int wavesSurvived;

    public GameData gameData;
    public PlayerPreferences playerPref;
    public SceneLoad sceneLoad;
    public Player player;

    [Header("Zombie")]
    public ZombieData zombieData;
    public ZombieSpawner zombieSpawner;
    public float spawnTime;

    [Header("Wave")]
    public int wave;
    public int targetKills;
    public int timer;
    [Tooltip("In seconds")] public int minTimer;
    [Tooltip("In seconds")] public int maxTimer;

    [Header("UI")]
    public WaveText waveText;
    public TimerText timerText;
    public KillCountText killCountText;

    [Header("Gun")]
    public GameObject gun;
    public GameObject gunPrefab;
    public GunData gunData;

    AudioSource audioSource;

    bool waveCompleted = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Get the gun prefab and gun data based on the selected gun index from the game data 
        gunPrefab = gameData.gunPrefabs[playerPref.LoadPrefs("Gun")];
        gunData = gunPrefab.GetComponent<Gun>().gunData;

        // Set the zombie's current speed to its mininum speed
        zombieData.speed = zombieData.minSpeed;

        wavesSurvived = 0;
        zombieKilled = 0;
    }

    void Start()
    {
        player = Player.instance;
        CreateGun();
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveCompleted && (zombieKilled >= targetKills || timerText.timerReached)) {
            waveCompleted = true;
            Intermission();
        }
        else if (waveCompleted && timerText.timerReached) {
            NextWave();
        }
    }

    // Instantiate the selected gun and parent it to the FPSController
    void CreateGun()
    {
        Transform parent = player.transform.GetChild(0);

        gun = Instantiate(gunPrefab, parent.transform);
        gun.transform.localPosition = gunData.normalLocalPosition;
    }

    void Intermission()
    {
        // Update UI
        waveText.Intermission();
        killCountText.Clear();

        // Start timer for intermission
        timerText.RestartTimer(20, true);
    }

    void NextWave()
    {
        // Increment wave number
        wave++;

        zombieKilled = 0;
        waveCompleted = false;

        // The initial target kill is 10 and is incremented by 5 every wave after
        targetKills = (wave-1)*5 + 10;

        // Change the timer, zombie speed, and zombie spawnTime every wave until it reaches its max value on wave 20
        timer = Mathf.Min(minTimer + (wave-1)*(maxTimer-minTimer)/19, maxTimer);
        zombieData.speed = Mathf.Min(zombieData.minSpeed + (wave-1)*(zombieData.maxSpeed-zombieData.minSpeed)/19, zombieData.maxSpeed);
        spawnTime = Mathf.Max(zombieSpawner.spawnTime - (wave-1)*(zombieSpawner.spawnTime-zombieSpawner.minSpawnTime)/19, zombieSpawner.minSpawnTime);

        // Restart timer for new wave
        timerText.RestartTimer(150);

        // Restart the zombie spawner and change the speed of any remaining zombies
        zombieSpawner.ChangeRemainingZombieSpeed(zombieData.speed);
        zombieSpawner.RestartSpawner(targetKills, spawnTime);

        // Play the drum sound at the beginning of the wave
        audioSource.Stop();
        audioSource.Play();

        // Update UI
        waveText.UpdateWave(wave);
        killCountText.SetTargetKill(targetKills);
        killCountText.UpdateKillCount();
    }

    public void GameOver()
    {
        wavesSurvived = wave;
        sceneLoad.LoadGameOverScene();
    }
}
