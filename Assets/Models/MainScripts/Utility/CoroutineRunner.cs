using UnityEngine;
using System;
using System.Collections;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CoroutineRunner");
                instance = go.AddComponent<CoroutineRunner>();
            }
            return instance;
        }
    }
}
