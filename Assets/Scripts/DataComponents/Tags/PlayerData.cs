using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerData: IComponentData
{
    public int lives;
    public float speed;
    public float rotationSpeed;
    public float intervalShootTime;
    public bool alreadyShoot;
    public bool upgradeToSuperLaser;
    public bool isForceShieldActive;
}
