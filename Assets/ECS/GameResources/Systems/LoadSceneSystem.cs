using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
public partial struct LoadSceneSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if there is a LevelResources entity
        /*if (SystemAPI.HasSingleton<LevelResources>())
        {
            // Get the LevelResources component
            ref LevelResources levelResources = ref SystemAPI.GetSingletonRW<LevelResources>().ValueRW;

            // Get the scene system
            SceneSystem sceneSystem = World.GetExistingSystem<SceneSystem>();

            // Load the scene
            sceneSystem.LoadSceneAsync(levelResources.Gamescene.SceneGUID);
        }*/
    }
}