using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager Instance { get; private set; }

    [System.Serializable]
    public class SetProceduralLegControllerEvent : UnityEvent<ProceduralLegController> { }

    public SetProceduralLegControllerEvent onSetProceduralLegController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetProceduralLegController(ProceduralLegController newReference)
    {
        onSetProceduralLegController.Invoke(newReference);
    }
}
