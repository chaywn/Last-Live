using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public NavMeshAgent agent;
    public Player player;
    [SerializeField] ZombieData zombieData;
    public KillCountText killCountText;

    Animator zombieAnimator;
    AudioSource audioSource;

    public int currentHealth;

    float attackTimer;

    bool playerInAttackRange;
    bool dead = false;
    bool attacked = false;
    bool fallBack = false;
    bool walking = false;

    void Start()
    {
        player = Player.instance;
        agent = GetComponent<NavMeshAgent>();
        killCountText = GameObject.Find("KillCountText").GetComponent<KillCountText>();
        zombieAnimator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        
        audioSource.volume = 0.4f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 20;
        
        agent.speed = zombieData.speed;
        currentHealth = zombieData.health;

        StartCoroutine(PlayAudio());
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        playerInAttackRange = (distance <= zombieData.attackRange) ? true : false;

        // Chase the player if the zombie is not dead and not falling backwards
        if (!dead && !fallBack) {
            ChasePlayer(distance);

            // Attack the player if within close range
            if (distance <= agent.stoppingDistance + 0.25f && !attacked) {

                // To prevent zombie from dealing damage as soon as it touches the player, 
                // the zombie has to remain close distance with the player for some time before actually dealing damage
                attackTimer += Time.deltaTime;
                if (attackTimer >= 0.2f) {
                    attacked = true;
                    StartCoroutine(AttackPlayer());
                }
            }
            else {
                attackTimer = 0;
            }
        }

        ControlAnimation();
    }


    void ChasePlayer(float distance)
    {
        walking = true;
        agent.SetDestination(player.transform.position);

        // Stop the zombie when it's very close to the player so it doesn't push the player
        if (distance <= agent.stoppingDistance) agent.SetDestination(transform.position);
    }


    IEnumerator AttackPlayer()
    {
        FaceTarget();

        int dmg = Mathf.RoundToInt(Random.Range(zombieData.damage.x, zombieData.damage.y));
        player.TakeDamage(dmg);

        yield return new WaitForSeconds(zombieData.attackRate);
        attacked = false;
    }
    void FaceTarget()
	{
		Vector3 direction = (player.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            // Increase the kill counter
            GameManager.zombieKilled++;

            // Refresh the UI to display the new kill count
            killCountText.UpdateKillCount();

            dead = true;
            StartCoroutine(Death());
        }
        // Push the zombie backwards if it's shot in close distance
        else if (playerInAttackRange && !fallBack) {
            Rigidbody rb = GetComponent<Rigidbody>();

            agent.ResetPath();
            fallBack = true;
            rb.AddForce(-transform.parent.transform.forward * 500);
            Invoke(nameof(ResetFallBack), 1.7f);
        }
    }
    void ResetFallBack()
    {
        fallBack = false;
    }

    IEnumerator Death()
    {
        GetComponent<Collider>().enabled = false;
        agent.SetDestination(transform.position);
        audioSource.volume = audioSource.volume / 2;

        yield return new  WaitForSeconds(4.6f);
        Destroy(transform.parent.gameObject);
    }


    // Change the animation state based on the boolean values
    void ControlAnimation()
    {
        zombieAnimator.SetBool("walking", walking);

        zombieAnimator.SetBool("playerInAttackRange", playerInAttackRange);

        zombieAnimator.SetBool("fallBack", fallBack);

        if (dead) zombieAnimator.SetTrigger("death");
    }


    // Play the zombie sounds
    IEnumerator PlayAudio()
    {
        while (true) {
            AudioClip randomClip = zombieData.audioClipArray[Random.Range(0, zombieData.audioClipArray.Length)];

            audioSource.PlayOneShot(randomClip);

            yield return new WaitForSeconds(randomClip.length);
        }
    }


    // Draw the range in the editor for better estimation
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zombieData.attackRange);
    }
}
