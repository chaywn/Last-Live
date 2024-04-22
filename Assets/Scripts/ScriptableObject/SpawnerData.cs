using UnityEngine;

[CreateAssetMenu(fileName="SpawnerData", menuName="ScriptableObject/Spawner", order=3)]
public class SpawnerData : ScriptableObject
{
    public GameObject[] prefabs;
    
    public Vector3[] spawnPoints;

    [Tooltip("Time between each spawn")]public float spawnTime;

    public float minimumSpawnTime;
}
