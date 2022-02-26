using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AsteroidData: IComponentData
{
    public int movementSpeed;
    public float3 movementDirection;
}
