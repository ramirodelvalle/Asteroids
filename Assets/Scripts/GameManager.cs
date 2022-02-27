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
    public GameObject asteroidPrefab;
    public GameObject laserPrefab;

    public Text scoreText;

    public int maxScore;
    public int cubesPerFrame;
    public float cubeSpeed = 3f;

    private int curScore;

    private Entity shipEntityPrefab;
    private Entity asteroidEntityPrefab;
    private Entity laserEntityPrefab;

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
        asteroidEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        laserEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(laserPrefab, settings);
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
    }

    private void Update()
    {
        
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
        float minValue = -6;
        float maxValue = 6;
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
        Entity newObjEntity = manager.Instantiate(asteroidEntityPrefab);

        manager.AddComponentData(newObjEntity, objTrans);

        float minForce = -2.5f;
        float maxForce = 2.5f;
        float3 direction = new float3(UnityEngine.Random.Range(minForce, maxForce),
            UnityEngine.Random.Range(minForce, maxForce), 0f);

        AsteroidData asteroid = new AsteroidData{
            movementSpeed = 1,
            movementDirection = direction
        };

        manager.AddComponentData(newObjEntity, asteroid);
    }

    public void SpawnLaser(Translation objTrans)
    {
        Entity newObjEntity = manager.Instantiate(laserEntityPrefab);
        //Entity newObjEntity = entityCommand.Instantiate(laserEntityPrefab);

        manager.AddComponentData(newObjEntity, objTrans);
        //entityCommand.AddComponent(newObjEntity, objTrans);

        float minForce = -2.5f;
        float maxForce = 2.5f;
        float3 direction = new float3(UnityEngine.Random.Range(minForce, maxForce),
            UnityEngine.Random.Range(minForce, maxForce), 0f);

        LaserData laser = new LaserData
        {
            movementSpeed = 1,
            movementDirection = direction
        };

        manager.AddComponentData(newObjEntity, laser);
        //entityCommand.AddComponent(newObjEntity, laser);
    }
}
