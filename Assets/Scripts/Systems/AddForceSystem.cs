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

        //SHIP
        Entities.ForEach
        ((ref PhysicsVelocity physicsVelocity, in PhysicsMass physicsMass, in Translation translation,
            in PlayerData player, in Rotation rotation, in LocalToWorld localToWorld) =>
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    PhysicsComponentExtensions.ApplyImpulse(ref physicsVelocity, physicsMass, translation, rotation,
                    localToWorld.Up * deltaTime * player.speed, translation.Value);
                }
            }).Run();

        //ASTEROID
        Entities.ForEach
        ((ref PhysicsVelocity physicsVelocity, in PhysicsMass physicsMass, in AsteroidData asteroid) =>
        {
            var forceVector = asteroid.movementDirection * asteroid.movementSpeed * deltaTime;
            physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
        }).Run();

        //LASER SHOOT
        Entities.ForEach
        ((ref PhysicsVelocity physicsVelocity, in Translation translation, in PhysicsMass physicsMass, in LaserData laser,
            in Rotation rotation, in LocalToWorld localToWorld) =>
        {
            var forceVector = laser.movementDirection * laser.movementSpeed * deltaTime;
            PhysicsComponentExtensions.ApplyImpulse(ref physicsVelocity, physicsMass, translation, rotation, 
                localToWorld.Up, translation.Value);
        }).Run();

        //UFO
        Entities.ForEach
        ((ref PhysicsVelocity physicsVelocity, in PhysicsMass physicsMass, in EnemyData enemyData) =>
        {
            var forceVector = enemyData.movementDirection * enemyData.movementSpeed * deltaTime;
            physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
        }).Run();
    }
}