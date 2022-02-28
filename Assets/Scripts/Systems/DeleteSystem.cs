using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(OnTriggerEnterSystem))]
public class DeleteSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        //ASTEROID
        Entities
            .WithAll<DeleteTag>()
            .WithStructuralChanges()
            .ForEach((Entity entity, in Translation translation, in AsteroidData asteroidData) =>
        {
            GameManager.instance.IncreaseScore();
            if (asteroidData.asteroidSize != 3)
            {
                GameManager.instance.SpawnAsteroid(translation, asteroidData);
            }
            commandBuffer.DestroyEntity(entity);
        }).Run();

        //ENEMY
        Entities
            .WithAll<DeleteTag, EnemyData>()
            .WithStructuralChanges()
            .ForEach((Entity entity, in Translation translation) =>
            {
                GameManager.instance.IncreaseScore();
                commandBuffer.DestroyEntity(entity);
            }).Run();

        Entities
            .WithAll<EnemyData>()
            .WithStructuralChanges()
            .ForEach((Entity entity, in Translation translation) =>
            {
                GameManager.instance.ShootUfo(translation);   
            }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }
}