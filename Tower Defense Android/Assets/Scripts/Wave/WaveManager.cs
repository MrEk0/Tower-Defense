using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[DefaultExecutionOrder(-100)]
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] List<WaveSettings> waves;

    private WaveSettings currentWave;
    private List<Enemy> currentEnemyList;
    private Transform startPoint;

    private float waveDuration;
    private float startTime;
    private float waveTime;
    private float timeBetweenSpawns;
    private float timeSinceWaveStarted = 0f;
    private float timeSinceEnemyDropped = Mathf.Infinity;

    private int waveNumber = 0;
    private int numberOfWaves;
    private int enemyNumberInWave = 0;
    private int numberOfEnemies = 0;
    private int numberOfDeactivatedEnemies = 0;

    private bool canSpawn = true;
    private Dictionary< int, List<Enemy>> waveOfEnemies = new Dictionary<int, List<Enemy>>();

    public event Action<int, int> onWaveChanged;

    private void Awake()
    {
        Instance = this;

        SetUpAllWaves();
        SetUpWaveSettings();

        numberOfWaves = waves.Count;
        startPoint = GameManager.GetStartPoint();
    }

    private void Start()
    {
        onWaveChanged(waveNumber, numberOfWaves);
    }

    private void SetUpAllWaves()
    {
        for(int i=0; i<waves.Count; i++)
        {
            List<Enemy> enemies=waves[i].GetAllWaveEnemies();
            numberOfEnemies += enemies.Count;
            waveOfEnemies.Add(i, enemies);
        }
    }

    private void Update()
    {
        if (GameManager.isGamePaused)
            return;

        ActivateEnemies();
        CheckWaveTime();
    }

    private void CheckWaveTime()
    {
        if (timeSinceWaveStarted > waveTime)
        {
            if (waveNumber < numberOfWaves - 1)
            {
                UpdateWave();
            }
            else//!!!!!
            {
                canSpawn = false;
                //gameover
            }
        }
        timeSinceWaveStarted += Time.deltaTime;
    }

    private void ActivateEnemies()
    {
        timeSinceEnemyDropped += Time.deltaTime;

        if (timeSinceWaveStarted >= startTime && canSpawn)
        {
            if (timeSinceEnemyDropped >= timeBetweenSpawns)
            {
                Enemy enemy = currentEnemyList[enemyNumberInWave];
                Profiler.BeginSample("INSTANTIATE");
                EnemyInstantiation(enemy);
                Profiler.EndSample();

                UIManager.Instance.InitializeHealthBar(enemy);

                enemyNumberInWave++;
                timeSinceEnemyDropped = 0f;
                timeSinceEnemyDropped += Time.deltaTime;//to avoid the situation when an enemy is not activated 
            }
        }
    }

    public void DeactivateEnemies(GameObject enemy)
    {
        enemy.SetActive(false);
        numberOfDeactivatedEnemies++;
        if(numberOfDeactivatedEnemies==numberOfEnemies && !GameManager.isGameOver)
        {
            UIManager.Instance.ShowWinPanel();
        }
    }

    public int GetNumberOfWaves()
    {
        return numberOfWaves;
    }

    public int GetNumberOfAllEnemies()
    {
        return numberOfEnemies;
    }

    private void EnemyInstantiation(Enemy enemy)
    {
        Transform enemyTransform = enemy.gameObject.transform;
        enemyTransform.position = startPoint.position;
        enemyTransform.rotation = Quaternion.identity;
        enemyTransform.parent = transform;
        enemyTransform.gameObject.SetActive(true);
    }

    private void UpdateWave()
    {
        waveNumber++;
        onWaveChanged(waveNumber, numberOfWaves);

        SetUpWaveSettings();

        timeSinceWaveStarted = 0f;
        enemyNumberInWave = 0;
    }

    private void SetUpWaveSettings()
    {
        currentWave = waves[waveNumber];
        currentEnemyList = waveOfEnemies[waveNumber];

        waveDuration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        waveTime = waveDuration + startTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;
    }
}
