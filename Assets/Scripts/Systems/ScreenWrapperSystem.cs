using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ScreenWrapperSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithNone<LaserData, EnemyLaserData>()
            .ForEach((ref Translation translation) =>
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

            if (translation.Value.x > 9 + range)
            {
                translation.Value.x = -9;
            }

            if (translation.Value.x < -9 - range)
            {
                translation.Value.x = 9;
            }
        }).Run();

        return default;
    }
}