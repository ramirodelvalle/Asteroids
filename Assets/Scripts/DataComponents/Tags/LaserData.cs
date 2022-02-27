using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct LaserData: IComponentData
{
    public int movementSpeed;
    public float3 movementDirection;
}
