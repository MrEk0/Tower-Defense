using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Profiling;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySettings enemyType;
    [SerializeField] Transform path;
    [SerializeField] float rotationSpeed=10f;

    private float _health;
    private float _speed;
    private float _damage;
    private float _angleOffset=180f;
    private int _wayPointNumber = 0;

    //Transform path;
    List<Transform> wayPoints;
    Vector2 newPos;
    Transform currentTarget;    
    Rigidbody2D rb;
    Transform myTransform;

    public event Action<float> onDamageTaken;
    public HealthBar HealthBar { private get; set; }
    

    private void Awake()
    {
        _health = enemyType.Health;
        _speed = enemyType.Speed;
        _damage = enemyType.Damage;

        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        wayPoints = CreateListOfWayPoint();
        currentTarget = wayPoints[_wayPointNumber];
    }

    //private void Start()
    //{
    //    HealthBar.Activate();
    //}

    private void OnBecameVisible()
    {
        HealthBar.SetMaxValue(_health);
        //HealthBar.Activate();
    }

    private List<Transform> CreateListOfWayPoint()
    {
        //path = GameManager.GetCurrentPath();
        List<Transform> wayPoints = new List<Transform>();

        foreach(Transform transform in path)
        {
            wayPoints.Add(transform);
        }

        return wayPoints;
    }

    private void Update()
    {
        float distance = Vector2.SqrMagnitude(currentTarget.position-myTransform.position);

        if (Mathf.Approximately(distance, 0))
        {
            if (_wayPointNumber < wayPoints.Count-1)
            {
                UpdateMovement();
            }
            else
            {
                UIManager.Instance.GetDamage(_damage);
                HealthBar.Deactivate();
                gameObject.SetActive(false);
            }
        }
        else
        {
            Profiler.BeginSample("FOLLOW");
            Move();
            HealthBar.FollowEnemy(myTransform);
            Profiler.EndSample();
        }
    }

    private void UpdateMovement()
    {
        _wayPointNumber++;
        currentTarget = wayPoints[_wayPointNumber];       
    }

    private void Move()
    {
        newPos = Vector2.MoveTowards(myTransform.position, currentTarget.position, _speed * Time.deltaTime);
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
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - _angleOffset);

        return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void TakeDamage(float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        //onDamageTaken(_health);
        HealthBar.ChangeSliderValue(_health);

        if (_health == 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            HealthBar.Deactivate();

            UIManager.Instance.ChangeNumberOfCoins(enemyType.GetRandomCoin());
        }
    }

    //public float GetHealth()
    //{
    //    return _health;
    //}
}
