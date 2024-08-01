using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithRoot : MonoBehaviour
{
    private Transform rootTransform;
    private Vector3 localPosition;

    void Start()
    {
        // Get the root transform of the character
        rootTransform = transform.parent;

        // Store the local position of the point relative to the root
        localPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        // Rotate the point along with the root
        transform.rotation = rootTransform.rotation;

        // Update the position of the point relative to the root
        transform.localPosition = rootTransform.TransformPoint(localPosition) - rootTransform.position;
    }
}
