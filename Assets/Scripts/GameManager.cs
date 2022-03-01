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

    public Text scoreText;

    public int maxScore;
    public int cubesPerFrame;
    public float cubeSpeed = 3f;
    private int curScore;
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
    }

    private void Start()
    {
        curScore = 0;

        DisplayScore();
        SpawnerEntitySystem.instance.SpawnShip();
        SpawnerEntitySystem.instance.SpawnRandomPositionAsteroids();
        SpawnerEntitySystem.instance.SpawnUfo();

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
            //SpawnerEntitySystem.instance.SpawnUfo();
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
}
