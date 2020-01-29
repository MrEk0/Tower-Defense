using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Wave", menuName ="Wave")]
public class WaveSettings : ScriptableObject
{
    [SerializeField] List<GameObject> listOfEnemies;
    //[SerializeField] Transform startPoint;
    [SerializeField] float duration;
    [SerializeField] float startSpawnTime = 3f;
    [SerializeField] float timeBetweenSpawns=1f;

    public float Duration => duration;
    public float StartSpawnTime => startSpawnTime;
    public float TimeBetweenSpawns => timeBetweenSpawns;
    //public Transform StartPoint => startPoint;

    public GameObject GetEnemy()
    {
        int numberOfEnemy = Random.Range(0, listOfEnemies.Count);
        return listOfEnemies[numberOfEnemy];
    }
}
