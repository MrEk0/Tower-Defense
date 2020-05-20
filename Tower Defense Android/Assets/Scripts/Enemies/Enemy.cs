using System.Collections.Generic;
using UnityEngine;


public enum EnemyClass
{
    Soldier,
    DesertSoldier,
    Stormtrooper,
    Tank,
    GreyAirplane,
    GreenAirplane
}
public class Enemy : MonoBehaviour
{
    [SerializeField] EnemySettings enemyType;
    [SerializeField] EnemyClass enemyClass;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float rotationSpeed=10f;

    private float health;
    private float speed;
    private float damage;
    private float angleOffset=180f;
    private int wayPointNumber = 0;

    private Transform path;
    private List<Transform> wayPoints;
    private Vector2 targetPos;
    private Transform currentTarget;
    private Rigidbody2D rb;
    private Transform myTransform;
    private GameObject explosion;

    public HealthBar HealthBar { private get; set; }
    

    private void Awake()
    {
        health = enemyType.Health;
        speed = enemyType.Speed;
        damage = enemyType.Damage;

        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        wayPoints = CreateListOfWayPoint();
        currentTarget = wayPoints[wayPointNumber];
        explosion = Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
        explosion.SetActive(false);
    }

    private void OnBecameVisible()
    {
        HealthBar.SetMaxValue(health);
    }

    private void SetUpAudio()
    {
        switch(enemyClass)
        {
            case EnemyClass.DesertSoldier:
                AudioManager.PlayDesertSoldierAudio();
                break;
            case EnemyClass.GreenAirplane:
                AudioManager.PlayGreenAirplaneAudio();
                break;
            case EnemyClass.GreyAirplane:
                AudioManager.PlayGreyAirplaneAudio();
                break;
            case EnemyClass.Soldier:
                AudioManager.PlaySoldierAudio();
                break;
            case EnemyClass.Stormtrooper:
                AudioManager.PlayStormtrooperAudio();
                break;
            case EnemyClass.Tank:
                AudioManager.PlayTankAudio();
                break;
        }
    }

    private List<Transform> CreateListOfWayPoint()
    {
        path = GameManager.GetCurrentPath();
        List<Transform> wayPoints = new List<Transform>();

        foreach(Transform transform in path)
        {
            wayPoints.Add(transform);
        }

        return wayPoints;
    }

    private void Update()
    {
        if (GameManager.isGamePaused || GameManager.isGameOver)
            return;

        SetUpAudio();
        ReachedWaypointBehaviour();
    }

    private void FixedUpdate()
    {
        if (GameManager.isGamePaused || GameManager.isGameOver)
            return;

        Move();
        HealthBar.FollowEnemy(myTransform);
    }

    private void ReachedWaypointBehaviour()
    {
        float distance = Vector2.SqrMagnitude(currentTarget.position - myTransform.position);

        if (Mathf.Approximately(distance, 0))
        {
            if (wayPointNumber < wayPoints.Count - 1)
            {
                UpdateMovement();
            }
            else
            {
                UIManager.Instance.GetDamage(damage);
                HealthBar.Deactivate();
                WaveManager.Instance.DeactivateEnemies(gameObject);
                AudioManager.StopEnemySound(enemyClass);
            }
        }
    }

    private void UpdateMovement()
    {
        wayPointNumber++;
        currentTarget = wayPoints[wayPointNumber];       
    }

    private void Move()
    {
        targetPos = Vector2.MoveTowards(myTransform.position, currentTarget.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(targetPos);

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

        return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void TakeDamage(float damage)
    {
        if (health == 0)
            return;

        health = Mathf.Max(health - damage, 0);
        HealthBar.ChangeSliderValue(health);
        SetUpHitAudio();

        if (health == 0)
        {
            Death();
        }
    }

    private void Death()
    {
        HealthBar.Deactivate();
        UIManager.Instance.ChangeNumberOfCoins(enemyType.GetRandomCoin());
        WaveManager.Instance.DeactivateEnemies(gameObject);

        explosion.transform.position = myTransform.position;
        explosion.SetActive(true);

        AudioManager.PlayEnemyExplosionAudio();
        AudioManager.StopEnemySound(enemyClass);
    }

    private void SetUpHitAudio()
    {
        switch(enemyClass)
        {
            case EnemyClass.DesertSoldier:
                AudioManager.PlayBodyHitAudio();
                break;
            case EnemyClass.Soldier:
                AudioManager.PlayBodyHitAudio();
                break;
            case EnemyClass.Stormtrooper:
                AudioManager.PlayBodyHitAudio();
                break;
            case EnemyClass.Tank:
                AudioManager.PlayBodyHitAudio();
                break;
            case EnemyClass.GreyAirplane:
                AudioManager.PlayEnemyMetalHitAudio();
                break;
            case EnemyClass.GreenAirplane:
                AudioManager.PlayEnemyMetalHitAudio();
                break;
        }
    }
}
