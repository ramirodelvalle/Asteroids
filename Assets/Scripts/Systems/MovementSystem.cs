using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.
            ForEach((ref Translation translation, ref PhysicsVelocity vel, ref Rotation rotation,
            in PlayerData player, in SpeedData speedData) =>
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(player.rotationSpeed * deltaTime)));
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(player.rotationSpeed * deltaTime * -1)));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //float3 value = translation.Value;
                //value += deltaTime * player.speed * math.forward(rotation.Value) *-1;
                //translation.Value = value;
            }
        }).Run();

        return default;
    }
}