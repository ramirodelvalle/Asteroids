using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(PickupSystem))] //chequeo si el delete tag esta en el objeto luego de aplicarlo al mismo
public class DeleteEntitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Allocator.TempJob); //could adds and removes components

        Entities
            .WithAll<DeleteTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                GameManager.instance.IncreaseScore();
                commandBuffer.DestroyEntity(entity);
            }).Run();
        commandBuffer.Playback(EntityManager);
        commandBuffer.Dispose();

        return default;
    }
}