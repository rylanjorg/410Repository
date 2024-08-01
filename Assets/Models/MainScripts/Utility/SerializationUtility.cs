using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Newtonsoft.Json;

public static class SerializationUtility 
{
    public static string GetPrefabPath(GameObject prefab)
    {
        if (prefab == null)
        {
            return string.Empty;
        }

        string path = UnityEditor.AssetDatabase.GetAssetPath(prefab);

        // Check if the prefab is in the Resources folder
        if (path.Contains("Resources"))
        {
            // Remove everything before "Resources/"
            path = path.Remove(0, path.IndexOf("Resources") + 10);

            // Remove the file extension
            path = path.Remove(path.IndexOf("."));

            return path;
        }
        else
        {
            Debug.LogError("Prefab is not in the Resources folder. Dynamic loading won't work during runtime.");
            return string.Empty;
        }
    }

    public static GameObject CreateGameObjectFromTransformData(TransformData transformData)
    {
        // Create a new GameObject
        GameObject newObject = new GameObject("RootTransform");

        // Apply the stored transform data to the new GameObject
        newObject.transform.position = transformData.position;
        newObject.transform.rotation = transformData.rotation;
        newObject.transform.localScale = transformData.scale;

        return newObject;
    }

    public static GameObject CreateGameObjectFromTransformData(TransformData transformData, string name, Transform parent, int layer)
    {
        // Create a new GameObject
        GameObject newObject = new GameObject(name);

        // Set the parent
        newObject.transform.parent = parent;

        // Debug log statement to display the transform data
        //Debug.Log($" New ObjectFromTransformData: {name} - Position: {transformData.position}, Rotation: {transformData.rotation.eulerAngles}, Scale: {transformData.scale}");

        // Apply the stored transform data to the new GameObject
        newObject.transform.localPosition = transformData.position;
        newObject.transform.localRotation = transformData.rotation;
        newObject.transform.localScale = transformData.scale;

        // Set the layer
        newObject.layer = layer;

        return newObject;
    }


   

    // Call this function to start the search from a specific GameObject
    public static HitBox FindHitBoxInTree(GameObject startObject)
    {
        // Keep track of visited objects to avoid infinite loop
        HashSet<GameObject> visitedObjects = new HashSet<GameObject>();

        // Start the recursive search
        return FindHitBoxRecursively(startObject, visitedObjects);
    }

    // Recursive function to search for HitBox in the GameObject tree
    private static HitBox FindHitBoxRecursively(GameObject currentObject, HashSet<GameObject> visitedObjects)
    {
        // Check if the current object has a HitBox script
        HitBox hitBox = currentObject.GetComponent<HitBox>();
        if (hitBox != null)
        {
            // Found the HitBox, return it
            return hitBox;
        }

        // Mark the current object as visited
        visitedObjects.Add(currentObject);

        // Check children
        for (int i = 0; i < currentObject.transform.childCount; i++)
        {
            GameObject child = currentObject.transform.GetChild(i).gameObject;

            // Check if the child has not been visited yet
            if (!visitedObjects.Contains(child))
            {
                HitBox hitBoxInChildren = FindHitBoxRecursively(child, visitedObjects);

                if (hitBoxInChildren != null)
                {
                    // Found the HitBox in children, return it
                    return hitBoxInChildren;
                }
            }
        }

        // Check siblings
        if (currentObject.transform.parent != null)
        {
            for (int i = 0; i < currentObject.transform.parent.childCount; i++)
            {
                GameObject sibling = currentObject.transform.parent.GetChild(i).gameObject;

                // Skip the current object
                if (sibling != currentObject && !visitedObjects.Contains(sibling))
                {
                    HitBox hitBoxInSiblings = FindHitBoxRecursively(sibling, visitedObjects);

                    if (hitBoxInSiblings != null)
                    {
                        // Found the HitBox in siblings, return it
                        return hitBoxInSiblings;
                    }
                }
            }
        }

        // HitBox not found in the current object and its children/siblings
        return null;
    }

    // Call this function to start the search from a specific GameObject
public static EnemyHealthBar FindEnemyHealthBarInTree(GameObject startObject)
{
    // Keep track of visited objects to avoid infinite loop
    HashSet<GameObject> visitedObjects = new HashSet<GameObject>();

    // Start the recursive search
    return FindEnemyHealthBarRecursively(startObject, visitedObjects);
}

// Recursive function to search for EnemyHealthBar in the GameObject tree
private static EnemyHealthBar FindEnemyHealthBarRecursively(GameObject currentObject, HashSet<GameObject> visitedObjects)
{
    // Check if the current object has an EnemyHealthBar script
    EnemyHealthBar enemyHealthBar = currentObject.GetComponent<EnemyHealthBar>();
    if (enemyHealthBar != null)
    {
        // Found the EnemyHealthBar, return it
        return enemyHealthBar;
    }

    // Mark the current object as visited
    visitedObjects.Add(currentObject);

    // Check children
    for (int i = 0; i < currentObject.transform.childCount; i++)
    {
        GameObject child = currentObject.transform.GetChild(i).gameObject;

        // Check if the child has not been visited yet
        if (!visitedObjects.Contains(child))
        {
            EnemyHealthBar enemyHealthBarInChildren = FindEnemyHealthBarRecursively(child, visitedObjects);

            if (enemyHealthBarInChildren != null)
            {
                // Found the EnemyHealthBar in children, return it
                return enemyHealthBarInChildren;
            }
        }
    }

    // Check siblings
    if (currentObject.transform.parent != null)
    {
        for (int i = 0; i < currentObject.transform.parent.childCount; i++)
        {
            GameObject sibling = currentObject.transform.parent.GetChild(i).gameObject;

            // Skip the current object
            if (sibling != currentObject && !visitedObjects.Contains(sibling))
            {
                EnemyHealthBar enemyHealthBarInSiblings = FindEnemyHealthBarRecursively(sibling, visitedObjects);

                if (enemyHealthBarInSiblings != null)
                {
                    // Found the EnemyHealthBar in siblings, return it
                    return enemyHealthBarInSiblings;
                }
            }
        }
    }

    // EnemyHealthBar not found in the current object and its children/siblings
    return null;
}
}



