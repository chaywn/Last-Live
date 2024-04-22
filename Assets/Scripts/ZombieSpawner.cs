using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] SpawnerData spawnerData;

    public float spawnTime;
    public float minSpawnTime;
    public int targetSpawnCount;
    public int spawnCount;

    void Start()
    {
        spawnTime = spawnerData.spawnTime;
        minSpawnTime = spawnerData.minimumSpawnTime;
    }

    void Spawn()
    {
        // Choose a random zombie to spawn from the list of prefabs (with different zombie variants)
        GameObject prefab = spawnerData.prefabs[Random.Range(0, spawnerData.prefabs.Length)];

        // Choose a random spawn location from the list of possible spawn points
        Vector3 spawnPosition = spawnerData.spawnPoints[Random.Range(0, spawnerData.spawnPoints.Length)];

        // Instantiate the zombie and parent it under the spawner gameObject
        GameObject child = Instantiate(prefab, spawnPosition, Quaternion.identity);
		child.transform.parent = transform;
        
        spawnCount ++;

        // End the spawning process when the target spawn count is reached
        if (spawnCount >= targetSpawnCount) {
            CancelInvoke();
        }
    }

    public void RestartSpawner(int target, float newSpawnTime)
    {
        // End the last spawning process (in case there is any)
        CancelInvoke();

        // Set the new targetSpawnCount and spawnTime
        targetSpawnCount = target;
        spawnTime = newSpawnTime;
        
        // Reset spawnCount and spawnCountReached
        spawnCount = 0;

        // Start the new spawning process
        InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
    }


    // Change the speed of the remaining zombies that are still alive
    public void ChangeRemainingZombieSpeed(float newSpeed)
    {
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                Zombie zombie = child.GetChild(0).GetComponent<Zombie>();
                zombie.agent.speed = newSpeed;
            }
        }
    }
}
