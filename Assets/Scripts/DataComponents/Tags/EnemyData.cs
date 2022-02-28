using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct EnemyData: IComponentData
{
    public int movementSpeed;
    public float3 movementDirection;
    public float secondsToFire;
    public bool alreadyShoot;
}