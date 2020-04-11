using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Wave", menuName ="Wave")]
public class WaveSettings : ScriptableObject
{
    [SerializeField] List<Enemy> listOfEnemies;
    [SerializeField] float duration;
    [SerializeField] float startSpawnTime = 3f;
    [SerializeField] float timeBetweenSpawns=1f;

    public float Duration => duration;
    public float StartSpawnTime => startSpawnTime;
    public float TimeBetweenSpawns => timeBetweenSpawns;

    public Enemy GetEnemy()
    {
        int numberOfEnemy = Random.Range(0, listOfEnemies.Count);
        return listOfEnemies[numberOfEnemy];
    }

    public List<Enemy> GetAllWaveEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();
        int numberOfEnemies = (int)(duration / timeBetweenSpawns) + 1;
        
        for(int i=0; i<numberOfEnemies; i++)
        {
            Enemy enemy = Instantiate(GetEnemy());
            enemy.gameObject.SetActive(false);
            enemies.Add(enemy);
        }

        return enemies;
    }
}
