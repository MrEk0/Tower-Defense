﻿using System;
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

    float waveDuration;
    float startTime;
    float waveTime;
    float timeBetweenSpawns;
    float timeSinceWaveStarted = 0f;
    float timeSinceEnemyDropped = Mathf.Infinity;

    int waveNumber = 0;
    int numberOfWaves;
    int enemyNumberInWave = 0;
    int numberOfEnemies = 0;
    int numberOfDeactivatedEnemies = 0;
    bool canSpawn = true;
    Dictionary< int, List<Enemy>> waveOfEnemies = new Dictionary<int, List<Enemy>>();

    public event Action<int, int> onWaveChanged;

    private void Awake()
    {
        Instance = this;

        SetUpAllWaves();
        SetUpWaveSettings();

        numberOfWaves = waves.Count;
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
        CheckWaveTime();

        ActivateEnemies();
    }

    private void CheckWaveTime()
    {
        timeSinceWaveStarted += Time.deltaTime;

        if (timeSinceWaveStarted > waveTime)
        {
            if (waveNumber < numberOfWaves - 1)
            {
                UpdateWave();
            }
            else
            {
                canSpawn = false;
                //gameover
            }
        }
    }

    private void ActivateEnemies()
    {
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

    public void DeactivateEnemies(GameObject enemy)
    {
        enemy.SetActive(false);
        numberOfDeactivatedEnemies++;
        //Debug.Log(numberOfEnemies);
        //Debug.Log(numberOfDeactivatedEnemies);
        if(numberOfDeactivatedEnemies==numberOfEnemies)
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
