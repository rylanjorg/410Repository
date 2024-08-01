using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class VFXSpawner : MonoBehaviour
{

    public ProceduralLegController proceduralLegController;
        
    void Awake()
    {
        PlayerInfo.Instance.OnInitalizeEventSystems.AddListener(HandleInitialization);
    }

    void HandleInitialization()
    {
        //Debug.Log("VFX Spawner Initialized by Initalization Event");
        AddListners();
    }

    void AddListners()
    {
        // Add a listener to the OnSpawnVFX event
        VFXEventController.Instance.OnSpawnVFXLocal.AddListener(SpawnVFXLocal);
        VFXEventController.Instance.OnSpawnVFXWorld.AddListener(SpawnVFXWorld);
        VFXEventController.Instance.OnSpawnSimpleVFXLocal.AddListener(SpawnSimpleVFXLocal);
        VFXEventController.Instance.OnSpawnSimpleVFXWorld.AddListener(SpawnSimpleVFXWorld);
        VFXEventController.Instance.OnSpawnVFXWorldProceduralLeg.AddListener(SpawnVFXWorldProceduralLeg);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------//
    // Weapon VFX Spawning
    // --------------------------------------------------------------------------------------------------------------------------------------------------//


    public void SpawnVFXLocal(int VFXIndex, Weapon weapon)
    {
        //Debug.Log("Spawn VFX");

        // Instantiate the VFX prefab
        //GameObject vfx = Instantiate(weapon.VFXStructs[VFXIndex].VfxPrefab, transform.position, weapon.WeaponModelInstance.transform.rotation);

        int flip = 0;
        // Flip the VFX if necessary
        if (flip == 1)
        {
            //vfx.transform.Rotate(180.0f, 0.0f, 0.0f);
        }
    }

    public void SpawnVFXWorld(int VFXIndex, Weapon weapon)
    {
        //Debug.Log("Spawn VFX");
        
        //Vector3 vfxPosition = weapon.VFXStructs[VFXIndex].transformData.position;
        ///vfxPosition.x += transform.position.x;
        //vfxPosition.z += transform.position.z;
        // Instantiate the VFX prefab
       // GameObject vfx = Instantiate(weapon.VFXStructs[VFXIndex].VfxPrefab, vfxPosition, Quaternion.identity);
        //vfx.transform.localScale = weapon.VFXStructs[VFXIndex].transformData.scale;

        int flip = 0;
        // Flip the VFX if necessary
        if (flip == 1)
        {
            //vfx.transform.Rotate(180.0f, 0.0f, 0.0f);
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------//
    // Procedural Leg VFX Spawning
    // --------------------------------------------------------------------------------------------------------------------------------------------------//


    public void SpawnVFXWorldProceduralLeg(int arrayIndex, int VFXIndex)
    {
        //Debug.Log("Spawn VFX");
        Vector3 vfxPosition = proceduralLegController.legs[arrayIndex].vfxStructs[VFXIndex].transformData.position;
        
        //Vector3 vfxPosition = weapon.VFXStructs[VFXIndex].transformData.position;
        vfxPosition.x += proceduralLegController.legs[arrayIndex].footTransform.position.x;
        vfxPosition.z += proceduralLegController.legs[arrayIndex].footTransform.position.z;
        // Instantiate the VFX prefab
        GameObject vfx = Instantiate(proceduralLegController.legs[arrayIndex].vfxStructs[VFXIndex].VfxPrefab, vfxPosition, Quaternion.identity);
        vfx.transform.localScale = proceduralLegController.legs[arrayIndex].vfxStructs[VFXIndex].transformData.scale;

        int flip = 0;
        // Flip the VFX if necessary
        if (flip == 1)
        {
            vfx.transform.Rotate(180.0f, 0.0f, 0.0f);
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------//
    // Player VFX Spawning
    // --------------------------------------------------------------------------------------------------------------------------------------------------//

  

    public void SpawnSimpleVFXLocal(VisualEffectStruct vfxHolder, Transform spawnLocation, Transform parentTransform = null)
    {
        //Debug.Log("Spawn VFX simple local");
        

        Transform vfxHolderTrans = WeaponExtensions.FindTransformInHierarchy(parentTransform, vfxHolder.parentTransformName);

        // Instantiate the VFX prefab
        GameObject vfx = Instantiate( vfxHolder.VfxPrefab, vfxHolderTrans.position,  Quaternion.identity);
        vfx.transform.parent = vfxHolderTrans;
        vfx.transform.localScale =  vfxHolder.transformData.scale;
        vfx.transform.localPosition += vfxHolder.transformData.position;

        
        
        
        // Set rotation to parent if it exists, otherwise use the holder's rotation
        if (parentTransform != null)
        {
            vfx.transform.localRotation = vfxHolder.transformData.rotation;
        }
        else
        {
            vfx.transform.localRotation = vfxHolder.transformData.rotation;
        }



        // Add destory logic
        switch(vfxHolder.vfxDestroyMode)
        {
            case  VisualEffectStruct.VFXDeathMode.DestoryAfterSetTime:
                KillVFXAfterTimer killVFXAfterTimer = vfx.AddComponent<KillVFXAfterTimer>();
                killVFXAfterTimer.lifeTime = vfxHolder.deathTime;
                break;
            case  VisualEffectStruct.VFXDeathMode.DestroyOnceNoParticles:
                // Add script
                KillVFX killVFX = vfx.AddComponent<KillVFX>();
                killVFX.killTime = vfxHolder.deathExitTime;
                killVFX.vfx = vfx.GetComponent<VisualEffect>();
                break;
            default:
                break;
        }

    }


    public void SpawnSimpleVFXWorld(VisualEffectStruct vfxHolder, Transform spawnLocation, Transform parentTransform = null)
    {
        //Debug.Log("Spawn VFX simple world");
        
       // Vector3 vfxPosition = vfxHolder.transformData.position;
        //vfxPosition.x += spawnLocation.position.x;
        //vfxPosition.z += spawnLocation.position.z;
        // Instantiate the VFX prefab
        GameObject vfx = Instantiate(vfxHolder.VfxPrefab, spawnLocation.position + vfxHolder.transformData.position, Quaternion.identity);
        vfx.transform.localScale =  vfxHolder.transformData.scale;
        vfx.transform.localRotation = vfxHolder.transformData.rotation;
        //vfx.transform.parent = spawnLocation;
        
        // Add destory logic
        switch(vfxHolder.vfxDestroyMode)
        {
            case  VisualEffectStruct.VFXDeathMode.DestoryAfterSetTime:
                KillVFXAfterTimer killVFXAfterTimer = vfx.AddComponent<KillVFXAfterTimer>();
                killVFXAfterTimer.lifeTime = vfxHolder.deathTime;
                break;
            case  VisualEffectStruct.VFXDeathMode.DestroyOnceNoParticles:
                // Add script
                KillVFX killVFX = vfx.AddComponent<KillVFX>();
                killVFX.killTime = vfxHolder.deathExitTime;
                killVFX.vfx = vfx.GetComponent<VisualEffect>();
                break;
            default:
                break;
        }
    }

}