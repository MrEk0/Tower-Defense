using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WaveManager : MonoBehaviour
{
    [SerializeField] List<WaveSettings> waves;
    [SerializeField] Transform startPoint;
    [SerializeField] TextMeshProUGUI waveText;

    WaveSettings currentWave;
    float duration;
    float startTime;
    float timeBetweenSpawns;
    //Transform startPoint;

    int waveNumber = 0;
    float timeSinceWaveStarted = 0f;
    float timeSinceEnemyDropped = Mathf.Infinity;
    bool canSpawn = true;

    

    private void Awake()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        currentWave = waves[waveNumber];

        duration = currentWave.Duration;
        startTime = currentWave.StartSpawnTime;
        timeBetweenSpawns = currentWave.TimeBetweenSpawns;
        //startPoint = currentWave.StartPoint;
        //startPoint = startPoint;
        waveText.text = "Wave "+(waveNumber+1).ToString() + "/" + waves.Count;

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
                Instantiate(currentWave.GetEnemy(), startPoint.position, Quaternion.identity, transform);
                timeSinceEnemyDropped = 0f;
            }           
        }

        timeSinceEnemyDropped += Time.deltaTime;
    }

    //private void StopSpawning()
    //{
    //    currentWave = null;
    //    duration = 0;
    //}
}
