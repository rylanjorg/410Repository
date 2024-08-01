using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/StateWrapper")]
[System.Serializable]
public class StateWrapper : ScriptableObject
{
    [SerializeReference]
    public State state;
}



