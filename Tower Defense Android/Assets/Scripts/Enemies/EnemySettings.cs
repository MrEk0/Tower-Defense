using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName ="Enemy")]
public class EnemySettings : ScriptableObject
{
    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float lowerCoinsBoundary;
    [SerializeField] float upperCoinsBoundary;

    public float Health => health;

    public float Speed => speed;

    public float Damage => damage;

    public int GetRandomCoin()
    {
       return Mathf.RoundToInt(Random.Range(lowerCoinsBoundary, upperCoinsBoundary));
    }
}
