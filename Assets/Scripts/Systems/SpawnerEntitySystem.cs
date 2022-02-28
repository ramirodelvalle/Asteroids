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

        //SHIP SHOOT
        Entities.WithAll<PlayerData>()
        .WithNone<SuperLaserTag>()
        .ForEach(
        (Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData,
            ref PlayerData player, ref Translation translation, in Rotation rotation) =>
        {
            if (!player.alreadyShoot)
            {
                var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
                ecb.AddComponent<LaserData>(entityInQueryIndex, newEntity);
                ecb.SetComponent(entityInQueryIndex, newEntity, translation);
                ecb.SetComponent(entityInQueryIndex, newEntity, rotation);

                player.alreadyShoot = true;
            }
        }).ScheduleParallel();

        //SHIP SUPER SHOOT
        Entities.WithAll<SuperLaserTag>().ForEach(
        (Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData,
            ref PlayerData player, ref Translation translation, in Rotation rotation) =>
        {
            if (!player.alreadyShoot)
            {
                var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn2);
                ecb.AddComponent<LaserData>(entityInQueryIndex, newEntity);
                ecb.SetComponent(entityInQueryIndex, newEntity, translation);
                ecb.SetComponent(entityInQueryIndex, newEntity, rotation);

                player.alreadyShoot = true;
            }
        }).ScheduleParallel();

        //UFO SHOOT
        //Entities.WithAll<EnemyLaserData>().ForEach(
        //(Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData,
        //    ref EnemyData enemy, ref Translation translation, in Rotation rotation) =>
        //{
        //    if (!enemy.alreadyShoot)
        //    {
        //        var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn);
        //        ecb.AddComponent<EnemyLaserData>(entityInQueryIndex, newEntity);
        //        ecb.SetComponent(entityInQueryIndex, newEntity, translation);
        //        ecb.SetComponent(entityInQueryIndex, newEntity, rotation);

        //        enemy.alreadyShoot = true;
        //    }
        //}).ScheduleParallel();

        //SHIP SHIELD
        //Entities.WithAll< PlayerData>().ForEach(
        //(Entity e, int entityInQueryIndex, ref EntitySpawnData spawnData,
        //    ref PlayerData player, ref Translation translation, in Rotation rotation) =>
        //{
        //    if (!player.alreadyShoot)
        //    {
        //        var newEntity = ecb.Instantiate(entityInQueryIndex, spawnData.EntityToSpawn2);
        //        ecb.AddComponent<LaserData>(entityInQueryIndex, newEntity);
        //        ecb.SetComponent(entityInQueryIndex, newEntity, translation);
        //        ecb.SetComponent(entityInQueryIndex, newEntity, rotation);

        //        player.alreadyShoot = true;

        //        ecb.
        //    }
        //}).ScheduleParallel();

        _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}