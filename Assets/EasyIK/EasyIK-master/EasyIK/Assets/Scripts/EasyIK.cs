using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;


public class EasyIK : MonoBehaviour
{
    [Header("IK properties")]
    [TabGroup("tab1", "General", TextColor = "green")] [SerializeField] public ProceduralAnimTargetController targetController;
    [TabGroup("tab1", "General")] [SerializeField] public int numberOfJoints = 2;
    [TabGroup("tab1", "General")] [SerializeField] public Transform ikTarget;
    [TabGroup("tab1", "General")] [SerializeField] public int iterations = 10;
    [TabGroup("tab1", "General")] [SerializeField] public float tolerance = 0.05f;
    [Title("Pole target (3 joint chain)")]
    [TabGroup("tab1", "General")] [SerializeField] public Transform poleTarget;
    [TabGroup("tab1", "General")] [SerializeField] public Transform movementBase;
    [TabGroup("tab1", "General")] [SerializeField] public List<Transform> meshTransforms = new List<Transform>();

    [Title("Debug:")]
    [TabGroup("tab1", "Debug", TextColor = "orange")] [SerializeField] public bool debugJoints = true;
    [TabGroup("tab1", "Debug")] [SerializeField] public bool localRotationAxis = false;
    [TabGroup("tab1", "Debug")] [SerializeField] [Range(0.0f, 1.0f)] public float gizmoSize = 0.05f;
    [TabGroup("tab1", "Debug")] [SerializeField] public bool poleDirection = false;
    [TabGroup("tab1", "Debug")] [SerializeField] public bool poleRotationAxis = false;
    

    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] private JointStruct[] jointStructs;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Vector3 startPosition;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Vector3[] jointPositions;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private float[] boneLength;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private float jointChainLength;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private float distanceToTarget;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Quaternion[] startRotation;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Quaternion[] initialRelativeMeshRotations;
   
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Vector3[] jointStartDirection;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Quaternion ikTargetStartRot;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Quaternion lastJointStartRot;

   

   
    

    // Remove this if you need bigger gizmos:
    


    //public bool staticEndJoint = false;

    public void OnAwake()
    {
        if(ikTarget == null)
        {
            //targetController.GetTarget();
        }

        // Let's set some properties
        jointChainLength = 0;
        jointStructs = new JointStruct[numberOfJoints];
        jointPositions = new Vector3[numberOfJoints];
        boneLength = new float[numberOfJoints];
        jointStartDirection = new Vector3[numberOfJoints];
        startRotation = new Quaternion[numberOfJoints];
        initialRelativeMeshRotations = new Quaternion[numberOfJoints];

        ikTargetStartRot = ikTarget.rotation;

        var current = this.GetComponent<JointStruct>();

        // For each bone calculate and store the lenght of the bone
        for (var i = 0; i < jointStructs.Length; i += 1)
        {
            if(current.jointTransform == null) current.SetTransform();
            jointStructs[i] = current;
            // If the bones lenght equals the max lenght, we are on the last joint in the chain
            if (i == jointStructs.Length - 1)
            {
                lastJointStartRot = current.jointTransform.rotation;
                return;
            }
            // Store length and add the sum of the bone lengths
            else
            {
                boneLength[i] = Vector3.Distance(current.jointTransform.position, current.jointTransform.GetChild(0).position);
                jointChainLength += boneLength[i];

                jointStartDirection[i] = current.jointTransform.GetChild(0).position - current.jointTransform.position;
                startRotation[i] = current.jointTransform.rotation;
            }
            // Move the iteration to next joint in the chain
            current = current.jointTransform.GetChild(0).GetComponent<JointStruct>();
            SetInitalMeshRotations(i);
        }
    }

    void Start()
    {
       PrintJointStructs();
    }

    void SetInitalMeshRotations(int index)
    {
        if (index == 0)
        {
            initialRelativeMeshRotations[index] = meshTransforms[index].rotation; //Quaternion.identity; // The first joint has no parent
        }
        else
        {
            initialRelativeMeshRotations[index] = meshTransforms[index].rotation; //Quaternion.Inverse(jointStructs[index - 1].jointTransform.rotation) * jointStructs[index].jointTransform.rotation;
        }
    }

    void PoleConstraint()
    {
        if (poleTarget != null && numberOfJoints < 4)
        {
            // Get the limb axis direction
            var limbAxis = (jointPositions[2] - jointPositions[0]).normalized;

            // Get the direction from the root joint to the pole target and mid joint
            Vector3 poleDirection = (poleTarget.position - jointPositions[0]).normalized;
            Vector3 boneDirection = (jointPositions[1] - jointPositions[0]).normalized;
            
            // Ortho-normalize the vectors
            Vector3.OrthoNormalize(ref limbAxis, ref poleDirection);
            Vector3.OrthoNormalize(ref limbAxis, ref boneDirection);

            // Calculate the angle between the boneDirection vector and poleDirection vector
            Quaternion angle = Quaternion.FromToRotation(boneDirection, poleDirection);

            // Rotate the middle bone using the angle
            jointPositions[1] = angle * (jointPositions[1] - jointPositions[0]) + jointPositions[0];
        }
    }

    void Backward()
    {
        // Iterate through every position in the list until we reach the start of the chain
        for (int i = jointPositions.Length - 1; i >= 0; i -= 1)
        {
            // The last bone position should have the same position as the ikTarget
            if (i == jointPositions.Length - 1)
            {
                jointPositions[i] = ikTarget.transform.position;
            }
            else if (!jointStructs[i].isStatic) // Adjust index to sample the correct jointStruct
            {
                jointPositions[i] = jointPositions[i + 1] + (jointPositions[i] - jointPositions[i + 1]).normalized * boneLength[i];
            }
            /*else if(jointStructs[i].isStatic && i != jointPositions.Length - 1)
            {
                jointPositions[i] = new Vector3(startPosition.x, jointPositions[i].y, startPosition.z);
            }*/

        }
    }

    void Forward()
    {
        // Iterate through every position in the list until we reach the end of the chain
        for (int i = 0; i < jointPositions.Length; i += 1)
        {
            // The first bone position should have the same position as the startPosition
            if (i == 0)
            {
                jointPositions[i] = startPosition;
            }
            else if (!jointStructs[i].isStatic)
            {
                jointPositions[i] = jointPositions[i - 1] + (jointPositions[i] - jointPositions[i - 1]).normalized * boneLength[i - 1];
            }
            
        }
    }
    
    public bool IsTargetReachable()
    {
        // Distance from the root to the ikTarget
        distanceToTarget = Vector3.Distance(jointPositions[0], ikTarget.position);

        // IF THE TARGET IS NOT REACHABLE
        if (distanceToTarget > jointChainLength)
        {
            return false;
        }
        else
        {
            return true;
        }
    }   

    public void SolveIK()
    {
        // Get the jointPositions from the joints
        for (int i = 0; i < jointStructs.Length; i += 1)
        {
            jointPositions[i] = jointStructs[i].jointTransform.position;
        }
        // Distance from the root to the ikTarget
        distanceToTarget = Vector3.Distance(jointPositions[0], ikTarget.position);

        // IF THE TARGET IS NOT REACHABLE
        if (distanceToTarget > jointChainLength)
        {
            // Direction from root to ikTarget
            var direction = ikTarget.position - jointPositions[0];

            // Get the jointPositions
            for (int i = 1; i < jointPositions.Length; i += 1)
            {
                if(!jointStructs[i].isStatic)
                {
                    jointPositions[i] = jointPositions[i - 1] + direction.normalized * boneLength[i - 1];
                }
                
            }
        }
        // IF THE TARGET IS REACHABLE
        else
        {
            // Get the distance from the leaf bone to the ikTarget
            float distToTarget = Vector3.Distance(jointPositions[jointPositions.Length - 1], ikTarget.position);
            float counter = 0;
            // While the distance to target is greater than the tolerance let's iterate until we are close enough
            while (distToTarget > tolerance)
            {
                startPosition = jointPositions[0];
                Backward();
                Forward();

                counter += 1;
                // After x iterations break the loop to avoid an infinite loop
                if (counter > iterations)
                {
                    break;
                }
            }
        }
        // Apply the pole constraint
        PoleConstraint();

        // Apply the jointPositions and rotations to the joints
        for (int i = 0; i < jointPositions.Length - 1; i += 1)
        {
            jointStructs[i].jointTransform.position = jointPositions[i];
            var targetRotation = Quaternion.FromToRotation(jointStartDirection[i], jointPositions[i + 1] - jointPositions[i]);
            jointStructs[i].jointTransform.rotation = targetRotation * startRotation[i];
        }
        // Let's constrain the rotation of the last joint to the IK target and maintain the offset.
        Quaternion offset = lastJointStartRot * Quaternion.Inverse(ikTargetStartRot);
        jointStructs.Last().jointTransform.rotation = ikTarget.rotation * offset;

        UpdateMeshRotations();
    }


    void UpdateMeshRotations()
    {
        for (var i = 0; i < jointStructs.Length; i ++)
        {
            //Debug.Log(i);
            if(meshTransforms[i] == null) continue;
            if (i == 0)
            {
                // Apply the base rotation first, then the local rotation
                //meshTransforms[i].rotation = movementBase.rotation; // * initialRelativeMeshRotations[i];
            }
            else
            {
                // Apply the parent's rotation first, then the local rotation
                //meshTransforms[i].rotation = movementBase.rotation; //jointStructs[i - 1].jointTransform.rotation * initialRelativeMeshRotations[i];
            }
        }
}

    void Update()
    {
       
        //SolveIK();
    }
    public void PrintJointStructs()
    {
        for (int i = 0; i < jointStructs.Length; i++)
        {
            //Debug.Log("JointStruct " + i + ":");
            jointStructs[i].Print();
        }
    }

    // Visual debugging
    void OnDrawGizmos()
    {   
        if (debugJoints == true)
        {   
            var current = transform;
            var child = transform.GetChild(0);

            for (int i = 0; i < numberOfJoints; i += 1)
            {
                if (i == numberOfJoints - 2)
                {
                    var length = Vector3.Distance(current.position, child.position);
                    DrawWireCapsule(current.position + (child.position - current.position).normalized * length / 2, Quaternion.FromToRotation(Vector3.up, (child.position - current.position).normalized), gizmoSize, length, Color.cyan);
                    break;
                }
                else
                {
                    var length = Vector3.Distance(current.position, child.position);
                    DrawWireCapsule(current.position + (child.position - current.position).normalized * length / 2, Quaternion.FromToRotation(Vector3.up, (child.position - current.position).normalized), gizmoSize, length, Color.cyan);
                    current = current.GetChild(0);
                    child = current.GetChild(0);
                }
            }
        }

        if (localRotationAxis == true)
        {    
            var current = transform;
            for (int i = 0; i < numberOfJoints; i += 1)
            {
                if (i == numberOfJoints - 1)
                {
                    drawHandle(current);
                }
                else
                {
                    drawHandle(current);
                    current = current.GetChild(0);
                }
            }
        }

        var start = transform;
        var mid = start.GetChild(0);
        var end = mid.GetChild(0);

        if (poleRotationAxis == true && poleTarget != null && numberOfJoints < 4)
        {    
            Handles.color = Color.white;
            Handles.DrawLine(start.position, end.position);
        }

        if (poleDirection == true && poleTarget != null && numberOfJoints < 4)
        {    
            Handles.color = Color.grey;
            Handles.DrawLine(start.position, poleTarget.position);
            Handles.DrawLine(end.position, poleTarget.position);
        }

    }

    void drawHandle(Transform debugJoint)
    {
        Handles.color = Handles.xAxisColor;
        Handles.ArrowHandleCap(0, debugJoint.position, debugJoint.rotation * Quaternion.LookRotation(Vector3.right), gizmoSize, EventType.Repaint);
                
        Handles.color = Handles.yAxisColor;
        Handles.ArrowHandleCap(0, debugJoint.position, debugJoint.rotation * Quaternion.LookRotation(Vector3.up), gizmoSize, EventType.Repaint);

        Handles.color = Handles.zAxisColor;
        Handles.ArrowHandleCap(0, debugJoint.position, debugJoint.rotation * Quaternion.LookRotation(Vector3.forward), gizmoSize, EventType.Repaint);
    }

     public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
     {
        Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);

            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);

            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
         }
     }
}
