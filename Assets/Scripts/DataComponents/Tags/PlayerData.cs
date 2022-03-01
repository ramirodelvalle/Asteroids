using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerData: IComponentData
{
    public float speed;
    public float rotationSpeed;
    public bool upgradeToSuperLaser;
    public bool isForceShieldActive;
}
