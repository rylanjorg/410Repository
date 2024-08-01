using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion initialRelativeRotation;

    public Transform parentTransform; // The transform of the parent object

    void Start()
    {
        // Store the initial relative rotation
        initialRelativeRotation = Quaternion.Inverse(parentTransform.rotation) * transform.rotation;
    }

    void Update()
    {
        // Apply the initial relative rotation
        transform.rotation = parentTransform.rotation * initialRelativeRotation;
    }
}