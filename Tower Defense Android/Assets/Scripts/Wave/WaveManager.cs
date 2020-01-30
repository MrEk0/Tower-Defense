using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class WaveManager : MonoBehaviour
{
    [SerializeField] List<WaveSettings> waves;
    [SerializeField] Transform startPoint;
    //[SerializeField] TextMeshProUGUI waveText;

    WaveSettings currentWave;
    float duration;
    float startTime;
    float timeBetweenSpawns;
    //Transform startPoint;

    int waveNumber = 0;
    float timeSinceWaveStarted = 0f;
    float timeSinceEnemyDropped = Mathf.Infinity;
    bool canSpawn = true;

    public event Action<int, int> onWaveChanged;

    private void Awake()
    {
        //UpdateWave();
        currentWave = waves[waveNumber];

        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;
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
                FindObjectOfType<UIManager>().CreateHealthBar(enemy);
                timeSinceEnemyDropped = 0f;
            }           
        }

        timeSinceEnemyDropped += Time.deltaTime;
    }
}
