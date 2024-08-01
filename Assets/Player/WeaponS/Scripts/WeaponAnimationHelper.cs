using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationHelper : MonoBehaviour
{
    public Transform target;
    
    public float speed;

    // Update is called once per frame
    public bool isEnabled = false; // Start with the action disabled

    void Awake()
    {
        
        isEnabled = false; // Ensure isEnabled is set to false
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Check if the '1' key has been pressed
        {
            isEnabled = !isEnabled; // Toggle isEnabled
        }

        if (isEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
