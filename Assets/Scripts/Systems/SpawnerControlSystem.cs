using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class SpawnerControlSystem : SystemBase
{
    private EndInitializationEntityCommandBufferSystem _endInitializationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        _endInitializationEntityCommandBufferSystem = World.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        //var ecb = _endInitializationEntityCommandBufferSystem.CreateCommandBuffer();
        //var spawnerQuery = EntityManager.CreateEntityQuery(typeof(EntitySpawnData));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ecb.AddComponent<ShouldSpawnTag>(spawnerQuery);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //ecb.RemoveComponent<ShouldSpawnTag>(spawnerQuery);
        }
    }
}