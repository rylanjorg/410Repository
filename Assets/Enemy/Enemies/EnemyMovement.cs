using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Creates necessary variables
    public float attackRange;
    public float attackWaitTime = 1.04f;
    public float attackTime = 1.29f;
    public float damage = 10f;
    public AIDestinationSetter destinationSetter;
    public AIPath aiPath;
    public AudioSource audioSource;
    public AudioClip punch;

    private bool recentHit = false;
    private float timeLastAttacked;
    private Animator animator;

    // Start method
    private void Start()
    {
        timeLastAttacked = Time.time;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // FixedUpdate method
    void FixedUpdate()
    {
        // Checks if the hasAGroed bool is destinationSetter is true
        if (destinationSetter.hasAgroed)
        {
            // Sets isMoving in animator to true
            animator.SetBool("isMoving", true);
            // Used to set rotation without considering vertical axis
            Vector3 toPlayer = destinationSetter.target.position - transform.position;
            toPlayer.y = 0f; 
            Quaternion rotationToPlayer = Quaternion.LookRotation(toPlayer);
            transform.rotation = rotationToPlayer;
            // calculates distance between enemy and target
            float distanceFromTarget = Vector3.Distance(transform.position, destinationSetter.target.position);
            // checks if distanceFrom target is withing attackRange
            if (distanceFromTarget <= attackRange)
            {
                // Sets inRange in animator to true
                animator.SetBool("inRange", true);
                // If difference in time is between 0 and attackWaitTime, returns
                if (Time.time - timeLastAttacked >= 0 && Time.time - timeLastAttacked < attackWaitTime)
                {
                    return;
                }
                // if difference in time is between attackWaitTIme and attackTime
                else if(Time.time - timeLastAttacked >= attackWaitTime && Time.time - timeLastAttacked < attackTime)
                {
                    // checks if recentHit is false and if so, damages player, plays sound effect, and setes recentHit to false
                    if (!recentHit)
                    {
                        PlayerInfo.Instance.TakeDamage(damage);
                        audioSource.PlayOneShot(punch);
                        recentHit = true;
                    }
                }
                // Otherwise updates timeLastAttacked and sets recentHit to false
                else
                {
                    timeLastAttacked = Time.time;
                    recentHit = false;
                }
            }
            // Otherwise updates timeLastAttacked and sets inRange to false
            else
            {
                timeLastAttacked = Time.time;
                animator.SetBool("inRange", false);
            }
        }
        // Checks if the enemy is at it's spawn and if so disables all bools in animator
        if (destinationSetter.atStart)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("inRange", false);
        }
    }
}
