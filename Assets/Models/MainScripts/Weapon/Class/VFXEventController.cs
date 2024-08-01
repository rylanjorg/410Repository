using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(VFXSpawner))]
public class VFXEventController : MonoBehaviour
{
    // Define a new UnityEvent that takes an int parameter
    [System.Serializable]
    public class SpawnVFXEventLocal : UnityEvent<int, Weapon> { }
    public class SpawnVFXEventWorld : UnityEvent<int, Weapon> { }

    public class SpawnSimpleVFXEventLocal : UnityEvent<VisualEffectStruct, Transform, Transform> { }
    public class SpawnSimpleVFXEventWorld : UnityEvent<VisualEffectStruct, Transform, Transform> { }


    public class SpawnSimpleVFXEventGeneral : UnityEvent<VisualEffectStruct, Transform, Transform> { }

    public class SpawnVFXEventWorldProceduralLeg : UnityEvent<int,int> { }

    // Create an instance of the event
    public SpawnVFXEventLocal OnSpawnVFXLocal;
    public SpawnVFXEventWorld OnSpawnVFXWorld;


    public SpawnSimpleVFXEventLocal OnSpawnSimpleVFXLocal;
    public SpawnSimpleVFXEventWorld OnSpawnSimpleVFXWorld;
    
    public SpawnSimpleVFXEventGeneral OnSpawnSimpleVFXGeneral;



    public SpawnVFXEventWorldProceduralLeg OnSpawnVFXWorldProceduralLeg;

    // Singleton instance
    public static VFXEventController Instance { get; private set; }
    public VFXSpawner vfxSpawner;
    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        OnSpawnVFXLocal = new SpawnVFXEventLocal();
        OnSpawnVFXWorld = new SpawnVFXEventWorld();

        OnSpawnSimpleVFXLocal = new SpawnSimpleVFXEventLocal();
        OnSpawnSimpleVFXWorld = new SpawnSimpleVFXEventWorld();

        OnSpawnSimpleVFXGeneral = new SpawnSimpleVFXEventGeneral();

        OnSpawnVFXWorldProceduralLeg = new SpawnVFXEventWorldProceduralLeg();

        vfxSpawner = GetComponent<VFXSpawner>();
    }

    public void SpawnVFXLocal(int VFXIndex, Weapon weapon)
    {
        // Instead of directly spawning the VFX, invoke the event
        OnSpawnVFXLocal.Invoke(VFXIndex, weapon);
    }

    public void SpawnVFXWorld(int VFXIndex, Weapon weapon)
    {
        // Instead of directly spawning the VFX, invoke the event
        OnSpawnVFXWorld.Invoke(VFXIndex,weapon);
    }
    public void SpawnVFXWorldProceduralLeg(int arrayIndex, int VFXIndex)
    {
        // Instead of directly spawning the VFX, invoke the event
        OnSpawnVFXWorldProceduralLeg.Invoke(arrayIndex,VFXIndex);
    }

    public void SpawnSimpleVFXLocal(VisualEffectStruct vfxHolder, Transform spawnLocation, Transform parentTransform)
    {
        // Instead of directly spawning the VFX, invoke the event
        OnSpawnSimpleVFXLocal.Invoke(vfxHolder, spawnLocation, parentTransform);
    }

    public void SpawnSimpleVFXWorld(VisualEffectStruct vfxHolder, Transform spawnLocation, Transform parentTransform)
    {
        // Instead of directly spawning the VFX, invoke the event
        OnSpawnSimpleVFXWorld.Invoke(vfxHolder, spawnLocation, parentTransform);
    }

    public void SpawnSimpleVFXGeneral(VisualEffectStruct vfxHolder, Transform spawnLocation, Transform parentTransform)
    {
        Debug.Log("SpawnSimpleVFXGeneral ->" + vfxHolder + ":" + spawnLocation);
        //Transform spawnLocation = vfxHolder.spawnLocation;
        if(vfxHolder.instanceSpace == VisualEffectStruct.VFXInstanceSpace.World)
        {
            OnSpawnSimpleVFXWorld.Invoke(vfxHolder, spawnLocation, parentTransform);
        }
        else if(vfxHolder.instanceSpace == VisualEffectStruct.VFXInstanceSpace.Local)
        {
            OnSpawnSimpleVFXLocal.Invoke(vfxHolder, spawnLocation, parentTransform);
        }
    }


}