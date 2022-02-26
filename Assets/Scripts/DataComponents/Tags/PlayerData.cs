using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerData: IComponentData
{
    public int lives;
    public float speed;
    public float rotationSpeed;
}
