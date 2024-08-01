using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientTowardsPoint : MonoBehaviour
{
    public Transform gunTip; // The point on the object that should pass through the target
    public Transform targetPoint; // The target point in world space
    public new string name = "GunTip";

    void Update()
    {
        if(gunTip == null && transform.childCount > 0)
        {
            // Find the child with the name "gunTip"
           // Find the child named "gunTip"
          gunTip = FindChildByName(transform, name);
        }


        // Calculate the direction from the gun tip to the target point
        Vector3 directionToTarget = targetPoint.position - gunTip.position;

        // Calculate the rotation that aligns the object's forward direction with the direction to the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, transform.up);

        // Apply the rotation to the object
        transform.rotation = targetRotation;


    }

    // Helper method to find a child transform by name recursively
    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform foundChild = FindChildByName(child, name);
            if (foundChild != null)
                return foundChild;
        }
        return null;
    }
}