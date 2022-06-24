using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct EnemyData: IComponentData
{
    public int movementSpeed;
    public float3 movementDirection;
    public int secondsToFire;
    public bool alreadyShoot;
}