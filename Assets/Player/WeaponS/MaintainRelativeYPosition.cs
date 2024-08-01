using UnityEngine;
public class MaintainRelativeYPosition : MonoBehaviour
{
    public Transform referenceTransform; // Reference to the transform to maintain relative Y position to

    private Vector3 initialPositionOffset; // Initial position offset between reference and this object

    void Start()
    {
        // Calculate the initial position offset between reference and this object
        initialPositionOffset = referenceTransform.InverseTransformPoint(transform.position);
    }

    void LateUpdate()
    {
        // Get the current position of the reference transform
        Vector3 referencePosition = referenceTransform.position;

        // Calculate the target position for this object based on the reference position and initial offset
        Vector3 targetPosition = referencePosition + referenceTransform.TransformDirection(initialPositionOffset);

        // Preserve the original Y position of this object
        targetPosition.y = transform.position.y;

        // Apply the target position to this object
        transform.position = targetPosition;
    }
}
