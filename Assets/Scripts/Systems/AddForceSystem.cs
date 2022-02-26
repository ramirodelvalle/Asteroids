using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class AddForceSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach
        ((ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Translation translation,
            in PlayerData player, in Rotation rotation, in LocalToWorld localToWorld) =>
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    PhysicsComponentExtensions.ApplyImpulse(ref physicsVelocity, physicsMass, translation, rotation,
                    localToWorld.Up * deltaTime * player.speed, translation.Value);
                }
         }).Run();
    }
}