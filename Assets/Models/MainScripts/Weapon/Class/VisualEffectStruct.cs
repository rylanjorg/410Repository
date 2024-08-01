using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
[InlineProperty]
public class VisualEffectStruct 
{
    public enum VFXInstanceSpace
    {
        World,
        Local
    }

    public enum VFXDeathMode
    {
        DestoryAfterSetTime,
        DestroyOnceNoParticles
    }



    // General VFX Properties
    [TabGroup("tab1", "General", TextColor = "green")] [SerializeField] private GameObject vfxPrefab;
    [TabGroup("tab1", "General")] public string name;
    [TabGroup("tab1", "General")] public bool dontSpawnOnCooldown = false;
    [TabGroup("tab1", "General")] public TransformData transformData;
    [TabGroup("tab1", "General")] public VFXInstanceSpace instanceSpace;
    [TabGroup("tab1", "General")] [ShowIf("IsLocalSpace")] [SerializeField] public string parentTransformName;

   


    private bool IsLocalSpace()
    {
        return instanceSpace == VFXInstanceSpace.Local;
    }


    // Timing Properties
    [TabGroup("tab1", "Timing", TextColor = "orange")] [PropertyRange(0, 10)] public float spawnTimeOffset;
    [TabGroup("tab1", "Timing")] public VFXDeathMode vfxDestroyMode;
    [TabGroup("tab1", "Timing")] [ShowIf("IsDestoryAfterSetTime")] public float deathTime;

    private bool IsDestoryAfterSetTime()
    {
        return vfxDestroyMode == VFXDeathMode.DestoryAfterSetTime;
    }


    [TabGroup("tab1", "Timing")] [ShowIf("IsDestroyOnceNoParticles")] public float deathExitTime;
    private bool IsDestroyOnceNoParticles()
    {
        //return true;
        return vfxDestroyMode == VFXDeathMode.DestroyOnceNoParticles;
    }




    public VisualEffectStruct(GameObject vfxPrefab, float timeOffset)
    {
        this.vfxPrefab = vfxPrefab;
        this.spawnTimeOffset = timeOffset;
       //this.parentTransform_ = null;
    }

    public VisualEffectStruct(GameObject vfxPrefab, float timeOffset, Transform parentObject)
    {
        this.vfxPrefab = vfxPrefab;
        this.spawnTimeOffset = timeOffset;
       // this.parentTransform_ = parentObject;
    }

    public GameObject VfxPrefab
    {
        get { return vfxPrefab; }
        set { vfxPrefab = value; }
    }

    /*public VisualEffectStruct Clone()
    {
        // Create a new instance of VisualEffectStruct with the same properties
        /*VisualEffectStruct clone = new VisualEffectStruct(this.vfxPrefab, this.spawnTimeOffset);

        // If transformData is not null, create a new instance with the same properties
        if (this.transformData != null)
        {
            clone.transformData = this.transformData.Clone(); // Assume that TransformData has a Copy method
        }

        if(this.vfxPrefab != null)
        {
            Debug.Log($"Cloning VFX prefab: {this.vfxPrefab.name}");
            Debug.Log($"Cloned VFX prefab: {clone.vfxPrefab.name}");
        }

        clone.instanceSpace = this.instanceSpace;
        clone.spawnTimeOffset = this.spawnTimeOffset;
        clone.deathTime = this.deathTime;
        clone.vfxDestroyMode = this.vfxDestroyMode;
        clone.name = this.name;
        //clone.parentTransform_ = this.parentTransform_;
        clone.parentTransformName_ = this.parentTransformName_;
        clone.dontSpawnOnCooldown = this.dontSpawnOnCooldown;
        //clone.transformData = this.transformData;

        return clone;
    }*/

   
}