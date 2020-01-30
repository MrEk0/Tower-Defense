using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    [SerializeField] List<WaveSettings> waves;
    [SerializeField] Transform startPoint;

    WaveSettings currentWave;
    UIManager uIManager;

    float duration;
    float startTime;
    float timeBetweenSpawns;
    float timeSinceWaveStarted = 0f;
    float timeSinceEnemyDropped = Mathf.Infinity;

    int waveNumber = 0;
    bool canSpawn = true;

    public event Action<int, int> onWaveChanged;

    private void Awake()
    {
        currentWave = waves[waveNumber];

        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;

        uIManager = GetComponent<UIManager>();
    }

    private void Update()
    {
        timeSinceWaveStarted += Time.deltaTime;

        if (timeSinceWaveStarted > duration+startTime)
        {
            if (waveNumber < waves.Count - 1)
            {
                waveNumber++;
                UpdateWave();
            }
            else
            {
                canSpawn = false;
            }
        }

        if(timeSinceWaveStarted>=startTime && canSpawn)
        {
            if (timeSinceEnemyDropped > timeBetweenSpawns)
            {
                Enemy enemy=Instantiate(currentWave.GetEnemy(), startPoint.position, Quaternion.identity, transform);
                enemy.UIManager = uIManager;
                uIManager.CreateHealthBar(enemy);
                timeSinceEnemyDropped = 0f;
            }           
        }

        timeSinceEnemyDropped += Time.deltaTime;
    }

    private void UpdateWave()
    {
        currentWave = waves[waveNumber];

        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;

        onWaveChanged(waveNumber, waves.Count);

        timeSinceWaveStarted = 0f;
    }
}
