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
    public Text scoreText;

    public int maxScore;
    public int cubesPerFrame;
    public float cubeSpeed = 3f;

    private int curScore;
    private Entity shipEntityPrefab;
    private Entity asteroidEntityPrefab;
    private EntityManager manager; //para instanciar los objetos al juego
    private BlobAssetStore blobAssetStore;

    private bool insaneMode;
    private Entity cubeEntityPrefab;

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
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    private void Start()
    {
        curScore = 0;

        insaneMode = false;

        DisplayScore();
        SpawnShip();
        SpawnRandomPositionAsteroid();
    }

    private void Update()
    {
        if (!insaneMode && curScore >= maxScore)
        {
            insaneMode = true;
            StartCoroutine(SpawnLotsOfCubes());
        }
    }

    IEnumerator SpawnLotsOfCubes()
    {
        while (insaneMode)
        {
            for (int i = 0; i < cubesPerFrame; i++)
            {
                SpawnNewAsteroid();
            }
            yield return null;
        }
    }

    void SpawnNewAsteroid()
    {
        Entity newCubeEntity = manager.Instantiate(cubeEntityPrefab);

        Vector3 direction = Vector3.up;
        Vector3 speed = direction * cubeSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        manager.AddComponentData(newCubeEntity, velocity);
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

    void SpawnRandomPositionAsteroid()
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
        //manager.SetComponentData(asteroidEntityPrefab, new AsteroidData { movementDirection = new Vector3(2, 2, 0) });

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
}
