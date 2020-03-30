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
    [SerializeField] Transform startPoint;

    WaveSettings currentWave;
    List<Enemy> currentEnemyList;

    float duration;
    float startTime;
    float timeBetweenSpawns;
    float timeSinceWaveStarted = 0f;
    float timeSinceEnemyDropped = Mathf.Infinity;

    int waveNumber = 0;
    int enemyNumberInWave = 0;
    int numberOfEnemies = 0;
    bool canSpawn = true;
    Dictionary< int, List<Enemy>> waveOfEnemies = new Dictionary<int, List<Enemy>>();

    public event Action<int, int> onWaveChanged;

    private void Awake()
    {
        Instance = this;

        SetUpAllWaves();
        SetUpWaveSettings();
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
        timeSinceWaveStarted += Time.deltaTime;

        if (timeSinceWaveStarted > duration+startTime)
        {
            if (waveNumber < waves.Count-1)
            {
                waveNumber++;
                UpdateWave();
            }
            else
            {
                canSpawn = false;
                //gameover
            }
        }

        if (timeSinceWaveStarted >= startTime && canSpawn) 
        {
            if (timeSinceEnemyDropped > timeBetweenSpawns)
            {
                Enemy enemy = currentEnemyList[enemyNumberInWave];
                Profiler.BeginSample("INSTANTIATE");
                EnemyInstantiation(enemy);
                Profiler.EndSample();

                UIManager.Instance.InitializeHealthBar(enemy);

                enemyNumberInWave++;
                timeSinceEnemyDropped = 0f;
            }
        }

        timeSinceEnemyDropped += Time.deltaTime;
    }

    public int GetNumberOfAllEnemies => numberOfEnemies;

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
        SetUpWaveSettings();

        onWaveChanged(waveNumber, waves.Count);

        timeSinceWaveStarted = 0f;
        enemyNumberInWave = 0;
    }

    private void SetUpWaveSettings()
    {
        currentWave = waves[waveNumber];
        currentEnemyList = waveOfEnemies[waveNumber];

        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;
    }
}
