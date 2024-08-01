using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;


[System.Serializable]
public class LineOfSightCondition : TransitionCondition
{   
    // Choose between Math.Infinity and a set distance for the ray.
    public enum DistanceMode
    {
        Infinity,
        SetDistance
    }

    [BoxGroup("LineOfSightCondition")] public LayerMask filterShouldIgnore_layerMask;
    [BoxGroup("LineOfSightCondition")] public string ignoreTag = "";
    [BoxGroup("LineOfSightCondition")] [HideIf("inheritRootGameObject", true, animate: false)] public bool inheritRootGameObject = true;
    [BoxGroup("LineOfSightCondition")] [ReadOnly] public Transform characterRoot;
    [BoxGroup("LineOfSightCondition")] public List<Transform> targets = new List<Transform>();
    [BoxGroup("LineOfSightCondition")] public LayerMask interactLayerMask;
    [BoxGroup("LineOfSightCondition")] public DistanceMode distanceMode;
    [BoxGroup("LineOfSightCondition")] [ShowIf("IsSetDistanceMode")] public float maxDistance = 10f;


    public void SetReferences(Transform characterRoot, LayerMask enemyTargetLayerMask)
    {
        this.characterRoot = characterRoot;
        interactLayerMask = enemyTargetLayerMask;
    }

    private bool IsSetDistanceMode()
    {
        return distanceMode == DistanceMode.SetDistance;
    }

    public override void OnStart()
    {
        targets = TransitionConditionUtility.GetPlayerTransforms();
    }

    // If distanceMode is set to SetDistance, this value will be used as the ray distance.
    public override bool IsMet()
    {
        bool isMet = false;
        foreach(Transform target in targets)
        {
            isMet = CheckLineOfSight(target);
            if(isMet)
            {
                return true;
            }
        }
        return false;
    }


    public bool CheckLineOfSight(Transform target)
    {

        // Cast a ray from the root in the direction of the target. Then filter by the layerMask.
        // If, for example, the ray intersects an object in the layermask before the target, then it will return false.
        // If the ray hits the target, return true; otherwise, return false.

        RaycastHit[] hits;
        float distance = (distanceMode == DistanceMode.Infinity) ? Mathf.Infinity : maxDistance;
        Vector3 dirToPlayer = target.transform.position - characterRoot.position;

        hits = RaycastUtility.RaycastAllWithSorting(characterRoot.position, dirToPlayer, distance, interactLayerMask);

        Debug.Log("casting rray");

        foreach (RaycastHit hit in hits)
        {

            if (hit.collider.gameObject == target.gameObject)
            {
                Debug.DrawRay(characterRoot.position, Vector3.Normalize(dirToPlayer) * hit.distance, Color.green);
                Debug.Log("Raycast hit target object: " + hit.collider.gameObject.name + " with layer: " + hit.collider.gameObject.layer);   
                return true;
            }   
            else if((filterShouldIgnore_layerMask == (filterShouldIgnore_layerMask | (1 << hit.collider.gameObject.layer))) || hit.collider.tag == ignoreTag)
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " with layer: " + hit.collider.gameObject.layer);
                continue;
            }
            else if(hit.distance > dirToPlayer.magnitude)
            {
                continue;
            }
            else
            {
                Debug.Log("Raycast hit break object: " + hit.collider.gameObject.name + " with layer: " + hit.collider.gameObject.layer);
                break;
            }
        }

        Debug.DrawRay(characterRoot.position, Vector3.Normalize(dirToPlayer) * distance, Color.red);
        return false;
    }

    public override void DrawGizmos()
    {
    }
}
