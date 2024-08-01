using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

[Serializable]
public struct GameResources : IComponentData
{
    public float currentGameDuration;
    public float currentGameDifficulty;
    public bool isInitalized;
    public Entity CharacterSpawnPointEntity;
    public Entity CharacterPrefabEntity;
    public Entity CameraPrefabEntity;
    public Entity PlayerPrefabEntity;
    
}
