using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] List<WaveSettings> waves;
    [SerializeField] Transform startPoint;

    WaveSettings currentWave;

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
        Instance = this;

        currentWave = waves[waveNumber];
        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;
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
            }
        }

        if(timeSinceWaveStarted>=startTime && canSpawn)
        {
            if (timeSinceEnemyDropped > timeBetweenSpawns)
            {
                Enemy enemy=Instantiate(currentWave.GetEnemy(), startPoint.position, Quaternion.identity, transform);
                UIManager.Instance.CreateHealthBar(enemy);
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
