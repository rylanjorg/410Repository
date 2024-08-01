using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    // Creates necessary variables
    [SerializeField] private LineRenderer laser;
    [SerializeField] private Transform laserStart;
    [SerializeField] private float maxLength;

    public float bobbingHeight = 0.2f;
    public float bobbingSpeed = 3.0f;
    public float damage = 2.0f;
    public EnemySpawning enemySpawn;
    public AIDestinationSetter destinationSetter;
    public AIPath aiPath;
    public float chargeTime = 5.0f;
    public float fireTime = 6.0f;
    public AudioSource audioSource;
    public AudioClip laserBeam;

    private float timeLastFired;
    private Vector3 startPos;

    // Start method
    void Start()
    {
        timeLastFired = Time.time;
        laser.enabled = false;
        startPos = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    // Update method
    void FixedUpdate()
    {
        // Runs HoverInPlace function
        HoverInPlace();
        // Checks if the hasAGroed bool is destinationSetter is true
        if (destinationSetter.hasAgroed)
        {
            // Makes the enemy look at the target
            transform.LookAt(destinationSetter.target);
            // Makes sure laser's starting point is always at laserStart position
            laser.SetPosition(0, laserStart.position);
            // If difference in time is between 0 and chargeTime, runs DeactivateLaser
            if (Time.time - timeLastFired >= 0 && Time.time - timeLastFired < chargeTime)
            {
                DeactivateLaser();
            }
            // If the difference in time is between chargeTime and fireTime
            else if (Time.time - timeLastFired >= chargeTime && Time.time - timeLastFired <= fireTime)
            {
                // If the laser isn't already active, activates laser and sets ending position of the laser and returns
                if (!laser.enabled)
                {
                    ActivateLaser();
                    Ray ray = new Ray(laserStart.position, laserStart.forward);
                    bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLength);
                    Vector3 hitPosition = cast ? hit.point : laserStart.position + laserStart.forward * maxLength;
                    laser.SetPosition(1, hitPosition);
                    if(hit.transform.tag == "Player")
                    {
                        PlayerInfo.Instance.TakeDamage(damage);
                    }
                    return;
                }
                return;
            }
            // Otherwise updates timeLastFired and runs DeactivateLaser
            else
            {
                timeLastFired = Time.time;
                DeactivateLaser();
            }
        }
        // Otherwise runs DeactivateLaser
        else
        {
            DeactivateLaser();
        }
    }

    // HoverInPlace method
    private void HoverInPlace()
    {
        // Use a sine function to create a smooth bobbing motion
        float yOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        // Apply the bobbing motion to the Y position
        transform.position = new Vector3(transform.position.x, startPos.y + yOffset, transform.position.z);
    }

    // ActivateLaser method
    void ActivateLaser()
    {
        // Turns laser on
        laser.enabled = true;
        audioSource.PlayOneShot(laserBeam);
    }

    // DeactivateLaser method
    void DeactivateLaser()
    {
        // Turns laser off and sets default starting positions for laser
        laser.enabled = false;
        laser.SetPosition(0, laserStart.position);
        laser.SetPosition(1, laserStart.position);
    }
}