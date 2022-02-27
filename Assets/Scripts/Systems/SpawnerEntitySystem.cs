using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;

public class SpawnerEntitySystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var ecb = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
        var deltaTime = Time.DeltaTime;

        Entities.WithAll<PlayerData>().ForEach(
        (Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData,
            ref Translation translation, ref PlayerData player, in Rotation rotation) =>
        {
            //Debug.Log("spawnData.Timer: " + spawnData.Timer);
            //Debug.Log("deltaTime: " + deltaTime);
            //spawnData.Timer -= deltaTime;
            //if (spawnData.Timer <= 0)
            //{
            //    spawnData.Timer = spawnData.SpawnDelay;
            //    player.alreadyShoot = false;
            //}

            if (!player.alreadyShoot)
            {
                var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                ecb.AddComponent<LaserData>(entityInQueryIndex, newEntity);
                ecb.SetComponent(entityInQueryIndex, newEntity, translation);
                ecb.SetComponent(entityInQueryIndex, newEntity, rotation);

                player.alreadyShoot = true;
            }

           
        }).ScheduleParallel();
        _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}