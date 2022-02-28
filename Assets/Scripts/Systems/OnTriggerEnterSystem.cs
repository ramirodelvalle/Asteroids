using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

public class OnTriggerEnterSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct OnTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PlayerData> allPlayers;
        [ReadOnly] public ComponentDataFromEntity<LaserData> allLasers;
        [ReadOnly] public ComponentDataFromEntity<AsteroidData> allAsteroids;
        [ReadOnly] public ComponentDataFromEntity<SuperLaserPowerUpData> allPowerUps;
        [ReadOnly] public ComponentDataFromEntity<EnemyData> allEnemies;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            if (allAsteroids.HasComponent(entityA) && allAsteroids.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("choque entre asteroides");
                return;
            }

            //Asteroid & player
            if (allPlayers.HasComponent(entityA) && allAsteroids.HasComponent(entityB))
            {
                PlayerData myType = allPlayers[entityA];
                if (myType.isForceShieldActive)
                {
                    //UnityEngine.Debug.Log("Force shield is active");
                    return;
                }
                //UnityEngine.Debug.Log("player Entity A: " + entityA + " choco con asteroide: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allAsteroids.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                PlayerData myType = allPlayers[entityB];
                if (myType.isForceShieldActive)
                {
                    //UnityEngine.Debug.Log("Force shield is active");
                    return;
                }
                //UnityEngine.Debug.Log("asteroide Entity A: " + entityA + " choco con player: " + entityB);
                entityCommandBuffer.AddComponent(entityA, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityB);
            }

            //Asteroid & laser
            if (allLasers.HasComponent(entityA) && allAsteroids.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("laser Entity A: " + entityA + " choco con asteroide: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allAsteroids.HasComponent(entityA) && allLasers.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("asteroide Entity A: " + entityA + " laser con laser: " + entityB);
                entityCommandBuffer.AddComponent(entityA, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityB);
            }

            //LASER & UFO
            if (allLasers.HasComponent(entityA) && allEnemies.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("laser Entity A: " + entityA + " choco con ufo: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allEnemies.HasComponent(entityA) && allLasers.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("Ufo Entity A: " + entityA + " choco con laser: " + entityB);
                entityCommandBuffer.AddComponent(entityA, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityB);
            }

            //PLAYER & UFO
            if (allPlayers.HasComponent(entityA) && allEnemies.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("player Entity A: " + entityA + " choco con ufo: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allEnemies.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("Ufo Entity A: " + entityA + " player con laser: " + entityB);
                entityCommandBuffer.AddComponent(entityA, new DeleteTag());
                entityCommandBuffer.DestroyEntity(entityB);
            }

            //PLAYER & POWER UP
            if (allPowerUps.HasComponent(entityA) && allPlayers.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("player Entity A: " + entityA + " choco con power up: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new SuperLaserTag());
                entityCommandBuffer.DestroyEntity(entityA);
            }
            else if (allPlayers.HasComponent(entityA) && allPowerUps.HasComponent(entityB))
            {
                //UnityEngine.Debug.Log("power up Entity A: " + entityA + " choco player: " + entityB);
                entityCommandBuffer.AddComponent(entityB, new SuperLaserTag());
                entityCommandBuffer.DestroyEntity(entityB);

                //entityCommandBuffer.Dispose();
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new OnTriggerSystemJob();
        job.allPlayers = GetComponentDataFromEntity<PlayerData>(true); //true para indicar que es readonly
        job.allLasers = GetComponentDataFromEntity<LaserData>(true);
        job.allAsteroids = GetComponentDataFromEntity<AsteroidData>(true);
        job.allPowerUps = GetComponentDataFromEntity<SuperLaserPowerUpData>(true);
        job.allEnemies = GetComponentDataFromEntity<EnemyData>(true);

        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld,
            inputDependencies);

        commandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
