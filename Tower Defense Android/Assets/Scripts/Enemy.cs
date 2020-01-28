using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySettings enemyType;
    [SerializeField] List<Transform> wayPoints;

    float health;
    float speed;
    float damage;

    Transform currentTarget;
    int wayPointNumber = 0;
    Rigidbody2D rb;

    private void Awake()
    {
        health = enemyType.Health;
        speed = enemyType.Speed;
        damage = enemyType.Damage;

        rb = GetComponent<Rigidbody2D>();
        currentTarget = wayPoints[wayPointNumber];
        wayPointNumber++;
    }

    private void Update()
    {
        if (!Mathf.Approximately(Vector2.Distance(transform.position, currentTarget.position), 0))
        {
            //Debug.Log(speed);
            Vector2 newPos=Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
            rb.MovePosition(newPos);
        }
        else if(wayPointNumber<wayPoints.Count)
        {
            currentTarget = wayPoints[wayPointNumber];
            wayPointNumber++;
        }
    }
}
