using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject shipPrefab;
    public GameObject asteroidPrefabBig;
    public GameObject asteroidPrefabMedium;
    public GameObject asteroidPrefabSmall;
    public GameObject powerUpPrefab;
    public GameObject ufoPrefab;
    public GameObject laserBallPrefab;

    public Text scoreText;

    public int maxScore;
    public int cubesPerFrame;
    public float cubeSpeed = 3f;

    private int curScore;

    private Entity shipEntityPrefab;
    private Entity asteroidBigEntityPrefab;
    private Entity asteroidMediumEntityPrefab;
    private Entity asteroidSmallEntityPrefab;
    private Entity powerUpEntityPrefab;
    private Entity ufoEntityPrefab;
    private Entity laserBallEntityPrefab;

    private EntityManager manager; //para instanciar los objetos al juego
    private BlobAssetStore blobAssetStore;

    public float timeToRespawnUfo = 15;
    public float seconds = 0;

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
        laserBallEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(laserBallPrefab, settings);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    private void Start()
    {
        curScore = 0;

        DisplayScore();
        SpawnShip();
        SpawnRandomPositionAsteroids();

        //SpawnTestAsteroid(); //TODO sacar
        //SpawnUfo();

        Translation translation = new Translation
        {
            Value = new float3(3, 0, 0)
        };
        //SpawnPowerUpSuperLaser(translation);
    }

    private void Update()
    {
        seconds += Time.deltaTime;
        if (seconds > timeToRespawnUfo)
        {
            SpawnUfo();
            timeToRespawnUfo = seconds + 15;
        }
    }

    public void IncreaseScore()
    {
        curScore++;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = "Score: " + curScore;
    }

    void SpawnRandomPositionAsteroids()
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

    void SpawnShip()
    {
        Entity newObjEntity = manager.Instantiate(shipEntityPrefab);
        Translation objTrans = new Translation
        {
            Value = new float3(0f, 0f, 0f)
        };

        manager.AddComponentData(newObjEntity, objTrans);
    }

    void SpawnAsteroid(Translation objTrans)
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
        for (int i = 0; i < 4; i++)
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
        if (seconds % 2 == 0)
        {
            Entity newObjEntity = manager.Instantiate(laserBallEntityPrefab);
            manager.AddComponentData(newObjEntity, translation);
            //manager.AddComponentData(newObjEntity, new SuperLaserPowerUpData());
        }
    }
}
