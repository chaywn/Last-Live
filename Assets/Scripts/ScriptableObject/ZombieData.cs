using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "ScriptableObject/Zombie", order=4)]
public class ZombieData : ScriptableObject
{
    public int health = 100;

    [Header("ZombieRange")]
    public float attackRange;

    [Header("ZombieAttack")]
    public Vector2 damage;
    [Tooltip("Duration for each attack (seconds)")]public float attackRate;

    [Header("Zombie Speed")]
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    [Header("ZombieAudio")]
    public AudioClip[] audioClipArray;

    
}
