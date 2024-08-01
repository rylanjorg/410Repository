using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;


public class GameResourcesAuthoring : MonoBehaviour
{
    [Header("Levels")] 
    //public List<LevelResources> Levels;

    public List<BakedSubSceneReference> Gamescene;
    //public BakedSubSceneReference GameLightingscene;

    
    [Header("Scene Initialization")] 
    public GameObject CharacterSpawnPointEntity;
    public GameObject CharacterPrefabEntity;
    public GameObject CameraPrefabEntity;
    public GameObject PlayerPrefabEntity;

    
    public class Baker : Baker<GameResourcesAuthoring>
    {
        public override void Bake(GameResourcesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameResources
            {
                CharacterSpawnPointEntity = GetEntity(authoring.CharacterSpawnPointEntity, TransformUsageFlags.Dynamic),
                CharacterPrefabEntity = GetEntity(authoring.CharacterPrefabEntity, TransformUsageFlags.Dynamic),
                CameraPrefabEntity = GetEntity(authoring.CameraPrefabEntity, TransformUsageFlags.Dynamic),
                PlayerPrefabEntity = GetEntity(authoring.PlayerPrefabEntity, TransformUsageFlags.Dynamic),
            });


            foreach (var level in authoring.Gamescene)
            {
                AddComponent(entity, new LevelResources
                {
                    Gamescene = level.GetEntitySceneReference(),
                });
            }
        }
    }
}