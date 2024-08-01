using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct GameDurationSystem : ISystem
{
    public void OnCreate(ref SystemState state) { 

    }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Queries for all Spawner components. Uses RefRW because this system wants
        // to read from and write to the component. If the system only needed read-only
        // access, it would use RefRO instead.
        foreach (RefRW<GameResources> resources in SystemAPI.Query<RefRW<GameResources>>())
        {
            // Update the game duration
            resources.ValueRW.currentGameDuration += SystemAPI.Time.DeltaTime;

        }
    }
}