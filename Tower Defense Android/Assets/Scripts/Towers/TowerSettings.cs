using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tower", menuName ="Tower")]
public class TowerSettings : ScriptableObject
{
    [SerializeField] float buildPrice;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] LayerMask enemyMask;

    public float BuildPrice => buildPrice;

    public GameObject Bullet => bullet;

    public float BulletSpeed  => bulletSpeed;

    public LayerMask EnemyMask => enemyMask;
}
