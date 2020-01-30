using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySettings enemyType;
    //[SerializeField] List<Transform> wayPoints;
    [SerializeField] Transform path;
    //[SerializeField] TextMeshProUGUI coinText;
    //[SerializeField] TextMeshProUGUI livesText;
    [SerializeField] bool isRightHandStart = true;

    float health;
    float speed;
    float damage;

    float angleOffset;

    List<Transform> wayPoints;
    Transform currentTarget;
    int wayPointNumber = 0;
    Rigidbody2D rb;

    float coins = 0f;

    private void Awake()
    {
        health = enemyType.Health;
        speed = enemyType.Speed;
        damage = enemyType.Damage;

        rb = GetComponent<Rigidbody2D>();
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
        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (Mathf.Approximately(distance, 0))
        {
            if (wayPointNumber < wayPoints.Count-1)
            {
                UpdateMovement();
            }
            else
            {
                //player get damage
                FindObjectOfType<UIManager>().GetDamage(damage);
                Destroy(gameObject);

                //coins += enemyType.GetRandomCoin();
                //coinText.text = "Coins " + coins.ToString();
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

        //Vector2 dir = currentTarget.position - transform.position;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - angleOffset, Vector3.forward);
        //transform.rotation = SmoothlyRotation(currentTarget);
        
    }

    private void Move()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
        rb.MovePosition(newPos);

        transform.rotation = SmoothRotation(currentTarget);
    }

    private Quaternion SmoothRotation(Transform target)
    {
        if (transform.position == target.position)
            return transform.rotation;

        Quaternion currentRotation = transform.rotation;

        Vector2 dir = currentTarget.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - angleOffset);

        return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10f);
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        if(health==0)
        {
            Destroy(gameObject);
            FindObjectOfType<UIManager>().ChangeNumberOfCoins(enemyType.GetRandomCoin());
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
