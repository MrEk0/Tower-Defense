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

    [SerializeField] GameObject bullet;

    public float BuildPrice => buildPrice;
    public float Range => range;
    public float ShootInterval => shootInterval;
    public float Damage => damage;

    public GameObject Bullet => bullet;
}
