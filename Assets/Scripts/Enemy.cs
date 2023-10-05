using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float attackDistance;
    public int damage;
    public int health;

    private bool isAttacking;
    private bool isDead;

    public NavMeshAgent agent;
    public Animator anim;
    bool shouldfollow;

    public float FollowRange=20;
    bool playable = true;
    private void Start()
    {
        playable = true;
    }
    void Update ()
    {
        // don't do anything if we're dead
        if(isDead)
            return;
        if(player == null)
            return; 
        if(player.GetComponent<Player>().death)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= FollowRange)
        {
            shouldfollow = true;
            if(playable)
            {
                SoundManager.Instance.Playmonster();
                playable = false;
            }
            
        }
        else
        {
            if(!playable)
            {
                SoundManager.Instance.stopMonsterSound();
                playable = true;
            }
            shouldfollow = false;
           


        }

        if (shouldfollow)
        {


            // are we able to attack the player?
            if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
            {
                agent.isStopped = true;

                if (!isAttacking)
                    Attack();
            }
            // if not, run after them
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                anim.SetBool("Running", true);
            }
        }
    }

    // called when we want to begin attacking the player
    void Attack ()
    {
        isAttacking = true;
        anim.SetBool("Running", false);
        anim.SetTrigger("Attack");

        Invoke("TryDamage", 1.3f);
        Invoke("DisableIsAttacking", 2.66f);
    }

    // called when we want to damage the player
    void TryDamage ()
    {
        // are we within range of the player?
        // if so - attack them
        if(Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            player.TakeDamage(damage);
        }
    }

    // called after the attack animation has played
    void DisableIsAttacking ()
    {
        isAttacking = false;
    }

    // called when the player attacks us
    public void TakeDamage (int damageToTake)
    {
        health -= damageToTake;

        // check for if the enemy needs to die
        if(health <= 0)
        {
            if (GameManager.instance.enemyGroup1Count > 0)
                GameManager.instance.enemyGroup1Count--;
            else
                GameManager.instance.enemyGroup2Count--;
            SoundManager.Instance.stopMonsterSound();
            isDead = true;
            agent.isStopped = true;
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 5f);
        }
    }

}