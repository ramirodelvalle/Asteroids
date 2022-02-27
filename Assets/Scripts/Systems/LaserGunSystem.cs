using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class LaserGunSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach
        ((ref PlayerData player) =>
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.alreadyShoot = false;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.alreadyShoot = true;
            }
        }).Run();
    }
}
