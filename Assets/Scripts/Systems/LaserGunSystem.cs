using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Transforms;
using Unity.Collections;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class LaserGunSystem : SystemBase
{
    EntityCommandBuffer cb;
    protected override void OnUpdate()
    {
        cb = new EntityCommandBuffer(Allocator.TempJob);

        Entities
            .WithAll<PlayerData>()
            .WithStructuralChanges()
            .ForEach((Entity entity, int entityInQueryIndex, ref PlayerData player, in Translation translation, in Rotation rotation) =>
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Entity newEntity = new Entity();
                    if (player.upgradeToSuperLaser)
                    {
                        newEntity = cb.Instantiate(SpawnerEntitySystem.instance.superLaserEntityPrefab);
                    }
                    else
                    {
                        newEntity = cb.Instantiate(SpawnerEntitySystem.instance.laserEntityPrefab);
                    }

                    cb.AddComponent(newEntity, translation);
                    cb.AddComponent(newEntity, rotation);
                }
            }).Run();
        cb.Playback(EntityManager);
        cb.Dispose();
    }
}
