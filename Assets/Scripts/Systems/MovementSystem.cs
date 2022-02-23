using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        float2 curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entities.ForEach((ref PhysicsVelocity vel, in SpeedData speedData) => 
        {
            float2 newVelocity = vel.Linear.xz;

            newVelocity += curInput * speedData.speed * deltaTime;

            vel.Linear.xz = newVelocity;
        }).Run();

        return default;
    }
}
