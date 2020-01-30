using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySettings enemyType;
    [SerializeField] Transform path;
    [SerializeField] bool isRightHandStart = true;

    float health;
    float speed;
    float damage;

    float angleOffset;
    int wayPointNumber = 0;
    float coins = 0f;

    List<Transform> wayPoints;
    Transform currentTarget;    
    Rigidbody2D rb;
    Transform myTransform;

    public UIManager UIManager { private get; set; }

    private void Awake()
    {
        health = enemyType.Health;
        speed = enemyType.Speed;
        damage = enemyType.Damage;

        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        wayPoints = CreateListOfWayPoint();
        currentTarget = wayPoints[wayPointNumber];

        angleOffset = isRightHandStart ? 180 : 0;
    }

    private List<Transform> CreateListOfWayPoint()
    {
        List<Transform> wayPoints = new List<Transform>();

        foreach(Transform transform in path)
        {
            wayPoints.Add(transform);
        }

        return wayPoints;
    }

    private void Update()
    {
        float distance = Vector2.Distance(myTransform.position, currentTarget.position);

        if (Mathf.Approximately(distance, 0))
        {
            if (wayPointNumber < wayPoints.Count-1)
            {
                UpdateMovement();
            }
            else
            {
                if (UIManager)
                {
                    UIManager.GetDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Move();           
        }
    }

    private void UpdateMovement()
    {
        wayPointNumber++;
        currentTarget = wayPoints[wayPointNumber];       
    }

    private void Move()
    {
        Vector2 newPos = Vector2.MoveTowards(myTransform.position, currentTarget.position, speed * Time.deltaTime);
        rb.MovePosition(newPos);

        myTransform.rotation = SmoothRotation(currentTarget);
    }

    private Quaternion SmoothRotation(Transform target)
    {
        if (myTransform.position == target.position)
            return myTransform.rotation;

        Quaternion currentRotation = myTransform.rotation;

        Vector2 dir = currentTarget.position - myTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - angleOffset);

        return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10f);
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            Destroy(gameObject);

            if (UIManager)
            {
                UIManager.ChangeNumberOfCoins(enemyType.GetRandomCoin());
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
