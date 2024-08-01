using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Script : MonoBehaviour
{
    // creates necessary variables
    public float attackRadius = 4.5f;
    public float damage = 10f;
    public float attackWaitTime = 3f;
    public float attackTime = 3.1f;
    public GameObject rightArm;
    public GameObject leftArm;
    public AIDestinationSetter destinationSetter;
    public AudioSource audioSource;
    public AudioClip groundSlam;

    private bool playSoundEffect = true;
    private bool recentHit = false;
    private float timeLastAttacked;
    private int _animIDAttackTrigger;
    private Animator animator;

    // Awake method
    void Awake()
    {
        // gets necessary components
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        AssignAnimationIDs();
    }

    // Start is called before the first frame update
    void Start()
    {
        timeLastAttacked = Time.time;
    }

    // AssignAnimationIDs method
    private void AssignAnimationIDs()
    {
        _animIDAttackTrigger = Animator.StringToHash("ArmSwing");
    }


    // FixedUpdate method
    void FixedUpdate()
    {
        // checks if hasAgroed is true
        if (destinationSetter.hasAgroed)
        {
            // Used to set rotation without considering vertical axis
            Vector3 toPlayer = destinationSetter.target.position - transform.position;
            toPlayer.y = 0f;
            Quaternion rotationToPlayer = Quaternion.LookRotation(toPlayer);
            transform.rotation = rotationToPlayer;
            float distanceFromTarget = Vector3.Distance(transform.position, destinationSetter.target.position);
            // checks if target is within attack range
            if (distanceFromTarget <= attackRadius - 2)
            {
                // if time is between 0 and attackWaitTime, returns
                if (Time.time - timeLastAttacked >= 0 && Time.time - timeLastAttacked < attackWaitTime)
                {
                    return;
                }
                // If difference in time is between attackWaitTime and attackTime, checks if sound effect has been played yet or not
                // and if so plays it, and checks if player has been hit recently and if not, damages player and plays animation
                else if (Time.time - timeLastAttacked >= attackWaitTime && Time.time - timeLastAttacked < attackTime)
                {
                    if(playSoundEffect)
                    {
                        audioSource.PlayOneShot(groundSlam);
                        playSoundEffect = false;
                    }
                    if (!recentHit)
                    {
                        CheckForPlayer(distanceFromTarget);
                        recentHit = true;

                        animator.SetTrigger(_animIDAttackTrigger);
                    }
                }
                // Otherwise updates timeLastAttacked and sets isAttacking in animator to false
                else
                {
                    timeLastAttacked = Time.time;
                    recentHit = false;
                    playSoundEffect = true;
                    damage = 10.0f;
                }
            }
        }
    }

    // CheckForPlayer method that takes in a float
    private void CheckForPlayer(float distanceFromTarget)
    {
        // creates arrays of colliders in a sphere around the boss
        distanceFromTarget = Mathf.Abs(distanceFromTarget);
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius);
        // checks if any of those colliders belong to the player
        foreach (Collider coll in colliders)
        {
            if (coll.gameObject.tag == "Player")
            {
                // if the player is within attackRadius, deals damage equal to initial damage - distanceFromTarget, otherwise damage is 0
                if (distanceFromTarget <= attackRadius)
                {
                    damage = damage - distanceFromTarget;
                }
                else
                {
                    damage = 0;
                }
                // rounds damage and damages player
                damage = Mathf.Round(damage);
                PlayerInfo.Instance.TakeDamage(damage);
            }
        }
    }
}
