using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Physics.Extensions;
using Unity.Collections;

public class SpawnerEntitySystem : MonoBehaviour
{
    public static SpawnerEntitySystem instance;

    public GameObject shipPrefab;
    public GameObject asteroidPrefabBig;
    public GameObject asteroidPrefabMedium;
    public GameObject asteroidPrefabSmall;
    public GameObject powerUpPrefab;
    public GameObject ufoPrefab;
    public GameObject laserPrefab;
    public GameObject superLaserPrefab;
    public GameObject laserBallPrefab;

    private Entity shipEntityPrefab;
    private Entity asteroidBigEntityPrefab;
    private Entity asteroidMediumEntityPrefab;
    private Entity asteroidSmallEntityPrefab;
    private Entity powerUpEntityPrefab;
    private Entity ufoEntityPrefab;
    public Entity laserEntityPrefab;
    public Entity superLaserEntityPrefab;
    public Entity laserBallEntityPrefab;

    private EntityManager manager; //para instanciar los objetos al juego
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        manager = World.DefaultGameObjectInjectionWorld.EntityManager; //inicializa el manager
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        shipEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(shipPrefab, settings);
        asteroidBigEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefabBig, settings);
        asteroidMediumEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefabMedium, settings);
        asteroidSmallEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefabSmall, settings);
        powerUpEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(powerUpPrefab, settings);
        ufoEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ufoPrefab, settings);
        laserEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(laserPrefab, settings);
        superLaserEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(superLaserPrefab, settings);
        laserBallEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(laserBallPrefab, settings);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    public void SpawnShip()
    {
        Entity newObjEntity = manager.Instantiate(shipEntityPrefab);
        Translation objTrans = new Translation
        {
            Value = new float3(0f, 0f, 0f)
        };

        manager.AddComponentData(newObjEntity, objTrans);
    }

    public void SpawnAsteroid(Translation objTrans)
    {
        Entity newObjEntity = manager.Instantiate(asteroidBigEntityPrefab);

        manager.AddComponentData(newObjEntity, objTrans);

        float minForce = -2.5f;
        float maxForce = 2.5f;
        float3 direction = new float3(UnityEngine.Random.Range(minForce, maxForce),
            UnityEngine.Random.Range(minForce, maxForce), 0f);

        AsteroidData asteroid = new AsteroidData
        {
            movementSpeed = 1,
            movementDirection = direction,
            asteroidSize = 1
        };

        manager.AddComponentData(newObjEntity, asteroid);
    }

    public void SpawnAsteroid()
    {
        Entity newObjEntity = manager.Instantiate(asteroidBigEntityPrefab);

        float minForce = -2.5f;
        float maxForce = 2.5f;
        float3 direction = new float3(UnityEngine.Random.Range(minForce, maxForce),
            UnityEngine.Random.Range(minForce, maxForce), 0f);

        Translation translation = new Translation
        {
            Value = new float3(-3, 0, 0)
        };
        manager.AddComponentData(newObjEntity, translation);

        AsteroidData asteroid = new AsteroidData
        {
            movementSpeed = 1,
            movementDirection = direction,
            asteroidSize = 1
        };

        manager.AddComponentData(newObjEntity, asteroid);
    }

    public void SpawnAsteroid(Translation translation, AsteroidData asteroidData)
    {
        for (int i = 0; i < 2; i++)
        {
            float minForce = -2.5f;
            float maxForce = 2.5f;
            float3 direction = new float3(UnityEngine.Random.Range(minForce, maxForce),
                UnityEngine.Random.Range(3, 6), 0f);

            Entity asteroidEntity = new Entity();

            AsteroidData asteroid = new AsteroidData
            {
                movementSpeed = 1,
                movementDirection = direction
            };

            if (asteroidData.asteroidSize == 1)
            {
                asteroid.asteroidSize = 2;
                asteroidEntity = asteroidMediumEntityPrefab;

            }
            else if (asteroidData.asteroidSize == 2)
            {
                asteroid.asteroidSize = 3;
                asteroidEntity = asteroidSmallEntityPrefab;
            }

            Entity newObjEntity = manager.Instantiate(asteroidEntity);
            manager.AddComponentData(newObjEntity, translation);
            manager.AddComponentData(newObjEntity, asteroid);
        }
    }

    public void SpawnRandomPositionAsteroids()
    {
        float minValue = -9;
        float maxValue = 9;
        for (int i = 0; i < 5; i++)
        {
            float x = UnityEngine.Random.Range(minValue, maxValue);
            float y = UnityEngine.Random.Range(minValue, maxValue);

            Translation translation = new Translation
            {
                Value = new float3(x, y, 0)
            };
            SpawnAsteroid(translation);
        }
    }

    public void SpawnUfo()
    {
        Entity newObjEntity = manager.Instantiate(ufoEntityPrefab);

        float min = -3.5f;
        float max = 3.5f;
        float3 direction = new float3(UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max), 0f);

        Translation translation = new Translation
        {
            Value = new float3(-10, UnityEngine.Random.Range(min, max), 0)
        };

        manager.AddComponentData(newObjEntity, translation);
    }

    public void SpawnPowerUpSuperLaser(Translation translation)
    {
        Entity newObjEntity = manager.Instantiate(powerUpEntityPrefab);
        manager.AddComponentData(newObjEntity, translation);
        manager.AddComponentData(newObjEntity, new SuperLaserPowerUpData());
    }

    public void ShootUfo(Translation translation)
    {
        //if (seconds % 2 == 0)
        //{
        //    Entity newObjEntity = manager.Instantiate(laserBallEntityPrefab);
        //    manager.AddComponentData(newObjEntity, translation);
        //    //manager.AddComponentData(newObjEntity, new SuperLaserPowerUpData());
        //}
    }
}