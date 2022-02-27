using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct EntitySpawnData : IComponentData
{
    public Entity EntityToSpawn;
    //public float SpawnDelay;
    //public float Timer;
}
