using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
public class JointStruct : MonoBehaviour
{
    public bool isStatic;
    public GameObject mesh;
    [ReadOnly] public Transform jointTransform;

    void Awake()
    {
        jointTransform = this.GetComponent<Transform>();
    }   

    public void SetTransform()
    {
        jointTransform = this.GetComponent<Transform>();
    }   


    /*public JointStruct Clone()
    {
        // Create a new instance of JointStruct with the same properties
        JointStruct clone = new JointStruct();
        clone.isStatic = this.isStatic;
        clone.mesh = this.mesh;
        clone.jointTransform = this.jointTransform;

        return clone;
    }*/

    public void Print()
    {
        Debug.Log($"IsStatic: {isStatic} , Mesh: {mesh} , JointTransform: {jointTransform}");
    }
}
