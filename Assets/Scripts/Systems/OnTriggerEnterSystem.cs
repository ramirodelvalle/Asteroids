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
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new OnTriggerSystemJob();
        job.allPlayers = GetComponentDataFromEntity<PlayerData>(true); //true para indicar que es readonly
        job.allLasers = GetComponentDataFromEntity<LaserData>(true); 
        job.allAsteroids = GetComponentDataFromEntity<AsteroidData>(true); 

        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld,
            inputDependencies);

        commandBufferSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
