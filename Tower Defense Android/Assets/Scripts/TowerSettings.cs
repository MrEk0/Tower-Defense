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
}
