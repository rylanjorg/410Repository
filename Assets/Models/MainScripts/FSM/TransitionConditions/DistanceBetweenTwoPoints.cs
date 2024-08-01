using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

using PlayerData;

[System.Serializable]
public class DistanceBetweenTwoPoints : TransitionCondition
{   
    public enum PointType
    {
        Player
    }


    [BoxGroup("DistanceBetweenTwoPoints")] public PointType pointType;
    [BoxGroup("DistanceBetweenTwoPoints")] public Transform characterRoot;
    [BoxGroup("DistanceBetweenTwoPoints")] public float distanceThreshold;
    [BoxGroup("DistanceBetweenTwoPoints")] [ReadOnly] public List<Transform> targets = new List<Transform>();
    [BoxGroup("DistanceBetweenTwoPoints")] [SerializeField] [ReadOnly] private float distance;

    public void SetReferences(Transform characterRoot)
    {
        this.characterRoot = characterRoot;
    }

    public void OnStart()
    {
        targets = TransitionConditionUtility.GetPlayerTransforms();
    }
    
    public override bool IsMet()
    {
        bool isMet = false;

        if(pointType == PointType.Player)
        {
            foreach(Transform target in targets)
            {
                distance = Vector3.Distance(characterRoot.position, target.position);
                bool localIsMet = distance <= distanceThreshold;
                if(localIsMet && !isMet)
                {
                    isMet = true;
                }
            }
        }

        if(isMet)
        {
            distance = 0;
        }
        return isMet;
    }

    public override void Reset()
    {
        distance = 0;
    }

}



