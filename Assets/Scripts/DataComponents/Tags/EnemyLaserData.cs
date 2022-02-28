using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct EnemyLaserData : IComponentData
{
    public int movementSpeed;
    public float3 movementDirection;
}
