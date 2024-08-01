using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStarter : MonoBehaviour
{
    public static CoroutineStarter Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartACoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void StopACoroutine(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
    }
}