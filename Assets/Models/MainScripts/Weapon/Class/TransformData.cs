using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale = Vector3.one;

    public TransformData Clone()
    {
        string json = JsonUtility.ToJson(this);
        TransformData clone = JsonUtility.FromJson<TransformData>(json);
        return clone;
    }
}

