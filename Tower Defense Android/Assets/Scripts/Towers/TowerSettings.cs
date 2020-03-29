using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tower", menuName ="Tower")]
public class TowerSettings : ScriptableObject
{
    [SerializeField] float buildPrice;
    [SerializeField] float range;
    [SerializeField] float shootInterval;
    [SerializeField] float damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject bullet;
    //[SerializeField] Transform castlePoint;
    [SerializeField] LayerMask enemyMask;

    public float BuildPrice => buildPrice;
    public float Range => range;
    public float ShootInterval => shootInterval;
    public float Damage => damage;

    public GameObject Bullet => bullet;

    public float BulletSpeed  => bulletSpeed;

    //public Transform CastlePoint  => castlePoint;
    public LayerMask EnemyMask => enemyMask;
}
