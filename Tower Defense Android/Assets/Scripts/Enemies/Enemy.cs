using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;


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
    //[SerializeField] Transform path;
    [SerializeField] float rotationSpeed=10f;

    private float _health;
    private float _speed;
    private float _damage;
    private float _angleOffset=180f;
    private int _wayPointNumber = 0;

    Transform path;
    List<Transform> wayPoints;
    Vector2 newPos;
    Transform currentTarget;    
    Rigidbody2D rb;
    Transform myTransform;
    GameObject explosion;

    //public event Action<float> onDamageTaken;
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
        explosion = Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
        explosion.SetActive(false);
    }

    private void OnBecameVisible()
    {
        HealthBar.SetMaxValue(_health);
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
        Profiler.BeginSample("AUDIO");
        SetUpAudio();
        Profiler.EndSample();
        ReachedWaypointBehaviour();
    }

    private void FixedUpdate()
    {
        if (GameManager.isGamePaused || GameManager.isGameOver)
            return;

        Profiler.BeginSample("FOLLOW");
        Move();
        Profiler.EndSample();
        HealthBar.FollowEnemy(myTransform);
    }

    private void ReachedWaypointBehaviour()
    {
        float distance = Vector2.SqrMagnitude(currentTarget.position - myTransform.position);

        if (Mathf.Approximately(distance, 0))
        {
            if (_wayPointNumber < wayPoints.Count - 1)
            {
                UpdateMovement();
            }
            else
            {
                UIManager.Instance.GetDamage(_damage);
                HealthBar.Deactivate();
                //wavemanager deactivate
                WaveManager.Instance.DeactivateEnemies(gameObject);
                Debug.Log("finish line");
                //gameObject.SetActive(false);
            }
        }
    }

    private void UpdateMovement()
    {
        _wayPointNumber++;
        currentTarget = wayPoints[_wayPointNumber];       
    }

    private void Move()
    {
        newPos = Vector2.MoveTowards(myTransform.position, currentTarget.position, _speed * Time.fixedDeltaTime);
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
        //if health =0?
        _health = Mathf.Max(_health - damage, 0);
        HealthBar.ChangeSliderValue(_health);
        SetUpHitAudio();

        if (_health == 0)
        {
            Debug.Log("health 0");
            HealthBar.Deactivate();
            AudioManager.PlayEnemyExplosionAudio();
            UIManager.Instance.ChangeNumberOfCoins(enemyType.GetRandomCoin());
            explosion.transform.position = myTransform.position;
            explosion.SetActive(true);
            WaveManager.Instance.DeactivateEnemies(gameObject);
            AudioManager.StopEnemySound(enemyClass);
        }
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
