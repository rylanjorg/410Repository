using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;


public struct LevelResources : IComponentData
{
    public EntitySceneReference Gamescene;
}