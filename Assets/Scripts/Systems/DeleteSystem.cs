using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

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
            .ForEach((Entity entity) =>
        {
            GameManager.instance.IncreaseScore();
            commandBuffer.DestroyEntity(entity);
        }).Run();

        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();
    }
}