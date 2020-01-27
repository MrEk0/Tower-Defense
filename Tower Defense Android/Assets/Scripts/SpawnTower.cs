using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTower : MonoBehaviour
{
    [SerializeField] GameObject greenTower;
    [SerializeField] GameObject redTower;
    [SerializeField] GameObject doubleRocketTower;
    [SerializeField] GameObject rocketTower;

    public Vector3 spawnPos { get; set; }

    public void SpawnGreenTower()
    {
        Instantiate(greenTower, spawnPos, Quaternion.identity);
    }
}
