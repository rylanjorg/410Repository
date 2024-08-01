using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayerData;

public static class TransitionConditionUtility
{
    public static GameObject FindObjectWithTag(GameObject startObject, string tag)
    {
        if (startObject == null)
        {
            Debug.LogError("Start object is null.");
            return null;
        }

        // Search in the entire hierarchy, including parent, children, and siblings.
        Transform[] allTransforms = startObject.transform.root.GetComponentsInChildren<Transform>(true);

        foreach (var transform in allTransforms)
        {
            if (transform.CompareTag(tag))
            {
                return transform.gameObject;
            }
        }

        Debug.LogWarning($"No object with tag '{tag}' found in the hierarchy.");
        return null;
    }

    public static void FindComponentWeaponModelAsync(GameObject startObject, Action<WeaponModel> callback)
    {
        // Perform asynchronous logic to find or create the weapon
        CoroutineRunner.Instance.StartCoroutine(FindComponentWeaponModelCoroutine(startObject, callback));
    }

    private static IEnumerator FindComponentWeaponModelCoroutine(GameObject startObject, Action<WeaponModel> callback)
    {
        // Simulate an asynchronous operation (you might replace this with actual asynchronous logic)
        yield return new WaitForSeconds(1);

        WeaponModel weapon = FindComponentWeaponModel(startObject);

        // Call the callback after the weapon has been created
        callback?.Invoke(weapon);
    }


    public static WeaponModel FindComponentWeaponModel(GameObject startObject)
    {
        if (startObject == null)
        {
            Debug.LogError("Start object is null.");
            return null;
        }

        // Search in the entire hierarchy, including parent, children, and siblings.
        Transform[] allTransforms = startObject.transform.root.GetComponentsInChildren<Transform>(true);

        foreach (var transform in allTransforms)
        {
            // Check if the object has the specified component
            WeaponModel weaponComponent = transform.GetComponent<WeaponModel>();
            if (weaponComponent != null)
            {
                return weaponComponent;
            }
        }

        Debug.LogWarning($"No object with WeaponModel component found in the hierarchy.");
        return null;
    }

    public static List<Transform> GetPlayerTransforms()
    {
        List<Transform> playerTransforms = new List<Transform>();

        foreach(PlayerRuntimeData playerRuntimeData in PlayerInfo.Instance.playerRuntimeDataList)
        {
            playerTransforms.Add(playerRuntimeData.generalData.playerTargetRoot.transform);
        }

        return playerTransforms;
    }

}
