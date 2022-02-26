using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ScreenWrapperSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref Translation translation, in PlayerData player, in LocalToWorld localToWorld) =>
        {
            float range = 0.5f;
            if (translation.Value.y > 5 + range)
            {
                translation.Value.y = -5;
            }

            if (translation.Value.y < -5 - range)
            {
                translation.Value.y = 5;
            }

            if (translation.Value.x > 5 + range)
            {
                translation.Value.x = -5;
            }

            if (translation.Value.x < -5 - range)
            {
                translation.Value.x = 5;
            }
        }).Run();
        return default;
    }
}