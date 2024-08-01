using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPositionIK : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1, 0); // Adjust as needed
    public ProceduralLegController proceduralLegController;
    public Quaternion bodyRotation;
    [SerializeField] private Transform bodyTransform;

    void Awake()
    {
        bodyTransform = transform;
    }

    void Update()
    {
        Vector3 newOffset = bodyTransform.parent.localPosition;
        offset = newOffset;

        Vector3 bodyPosition = CalculateBodyPosition(proceduralLegController.legs, offset);

        // Set the body position
        bodyTransform.position = bodyPosition;

        bodyRotation = CalculateBodyRotation(proceduralLegController.legs);
        //bodyTransform.rotation = bodyRotation;
    }

    Vector3 CalculateBodyPosition(List<ProceduralLeg> legs, Vector3 offset)
    {
        Vector3 sumPositions = Vector3.zero;
        for (int i = 0; i < legs.Count; i++)
        {
            sumPositions += legs[i].targetTransform.position;
        }

        Vector3 averagePosition = sumPositions / legs.Count;
        Vector3 bodyPosition = averagePosition + offset;

        return bodyPosition;
    }

    Quaternion CalculateBodyRotation(List<ProceduralLeg> legs)
    {
        // Construct a plane using the four leg positions
        Plane plane = new Plane(legs[0].targetTransform.position, legs[1].targetTransform.position, legs[2].targetTransform.position);

        // Get the normal of the plane
        Vector3 planeNormal = plane.normal;

        // If the normal is facing downwards, flip it
        if (planeNormal.y < 0)
        {
            planeNormal = -planeNormal;
        }

        // Convert the normal to a rotation
        Quaternion bodyRotation = Quaternion.FromToRotation(Vector3.up, planeNormal);

        Debug.DrawRay(bodyTransform.position, bodyRotation * Vector3.up, Color.magenta);

        return bodyRotation;
    }

  
}