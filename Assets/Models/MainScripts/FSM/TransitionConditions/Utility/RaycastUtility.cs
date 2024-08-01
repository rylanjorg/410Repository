using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RaycastUtility
{
    public static RaycastHit[] RaycastAllWithSorting(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask)
    {
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, distance, layerMask);
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
}
