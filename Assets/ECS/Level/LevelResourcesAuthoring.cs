using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

public class LevelResourcesAuthoring : MonoBehaviour
{
    public BakedSubSceneReference Gamescene;
    public class Baker : Baker<LevelResourcesAuthoring>
    {
        public override void Bake(LevelResourcesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new LevelResources
            {
                Gamescene = authoring.Gamescene.GetEntitySceneReference(),
                // Assign other properties as needed
            });
        }
    }
}