using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    //[SerializeField] GameObject greenTower;
    //[SerializeField] GameObject redTower;
    //[SerializeField] GameObject doubleRocketTower;
    //[SerializeField] GameObject rocketTower;

    public Vector3 spawnPos { private get; set; }
    public bool canSpawn { private get; set; } = true;

    public void SpawnTower(GameObject typeOfTower)
    {
        if(canSpawn)
        Instantiate(typeOfTower, spawnPos, Quaternion.identity);
    }

}
