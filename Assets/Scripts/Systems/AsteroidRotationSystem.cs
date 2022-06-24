using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class AsteroidRotationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation rotation, in RotationSpeedData rotationSpeed) =>
        {
            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(rotationSpeed.speed * deltaTime)));
        }).Run();
        return default;
    }
}
