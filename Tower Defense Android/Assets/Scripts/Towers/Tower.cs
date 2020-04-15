﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;
    [SerializeField] TowerClass towerClass;
    [SerializeField] TowerUpdate towerUpdate;

    int currentLevel = 1;
    float range;
    float shotInterval;
    float damage;
    float timeSinceLastShot = Mathf.Infinity;

    LayerMask enemyMask;
    //Transform lastWayPoint;
    Vector3 lastPointPosition;
    GameObject bulletPrefab;   
    Transform target;
    Transform myTransform;
    List<GameObject> bullets;

    private void Awake()
    {
        UpdateParameters();
        enemyMask = towerType.EnemyMask;
        bulletPrefab = towerType.Bullet;

        myTransform = GetComponent<Transform>();
        lastPointPosition = GameManager.GetLastPathPoint().position;//!!!!!

        CreateListOfBullets();
    }

    private void OnEnable()
    {
        UIManager.Instance.onTowerUpdated += UpdateTower;
    }

    private void OnDisable()
    {
        UIManager.Instance.onTowerUpdated -= UpdateTower;
    }

    //private void Start()
    //{
    //    lastWayPoint = GameManager.GetLastPathPoint();
    //}

    void Update()
    {
        if (GameManager.isGamePaused || GameManager.isGameOver)
            return;

        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Profiler.BeginSample("FONDTARGET");
            FindNewTarget();
            Profiler.EndSample();
        }
        else
        {
            if (Vector2.Distance(myTransform.position, target.transform.position) > range)
            //if(Vector2.SqrMagnitude(target.transform.position-myTransform.position)>range)//??????
            {
                FindNewTarget();
            }
            else
            {
                Rotate();
                Shoot();
            
            }
        }
    }

    private void UpdateParameters()
    {
        range = towerUpdate.GetRange(towerClass, currentLevel);
        shotInterval = towerUpdate.GetShotInterval(towerClass, currentLevel);
        damage = towerUpdate.GetDamage(towerClass, currentLevel);
    }

    private void CreateListOfBullets()
    {
        bullets = new List<GameObject>();
        int numberOfBullets = GetNumberOfActiveBullets();
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            CreateABullet();
        }
    }

    private void CreateABullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, myTransform.position, Quaternion.identity, myTransform);
        bullet.SetActive(false);
        bullets.Add(bullet);
    }

    private int GetNumberOfActiveBullets()
    {
        float timeToGetRange = range / towerType.BulletSpeed;
        return Mathf.CeilToInt(timeToGetRange/shotInterval);
    }

    //private void OnDrawGizmos()//delete
    //{
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}

    private void FindNewTarget()
    {
        float distanceToEnemy = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, range, enemyMask);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            //float currentDistance = Vector2.Distance(lastWayPoint.position, colliders[i].transform.position);
            float currentDistance = Vector2.SqrMagnitude(colliders[i].transform.position-lastPointPosition);
            if (currentDistance < distanceToEnemy)
            {
                distanceToEnemy = currentDistance;
                target = colliders[i].transform;
            }
        }
        //Collider2D collider = Physics2D.OverlapCircle(myTransform.position, range, enemyMask);
        //if(collider)
        //target = collider.transform;
    }

    private void Rotate()
    {
        Vector2 dir = target.position - myTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }



    private void Shoot()
    {
        if (timeSinceLastShot > shotInterval)
        {
            SetUpBullet();
            SetUpAudio();

            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private void SetUpBullet()
    {
        GameObject bullet = bullets.First(b => !b.activeInHierarchy);
        bullet.GetComponent<Bullet>().SetParameters(towerType.BulletSpeed, damage);
        bullet.transform.position = myTransform.position;
        bullet.transform.rotation = myTransform.rotation;
        bullet.GetComponent<Bullet>().target = target;
        bullet.SetActive(true);
    }

    private void SetUpAudio()
    {
        switch(towerClass)
        {
            case TowerClass.ClosedDoubleRocket:
                AudioManager.PlayRocketTowerFireAudio();
                break;
            case TowerClass.DoubleRocket:
                AudioManager.PlayRocketTowerFireAudio();
                break;
            case TowerClass.Green:
                AudioManager.PlayBulletTowerFireAudio();
                break;
            case TowerClass.Red:
                AudioManager.PlayBulletTowerFireAudio();
                break;
            case TowerClass.Rocket:
                AudioManager.PlayRocketTowerFireAudio();
                break;
        }
    }

    private void OnMouseDown()
    {       
        UIManager.Instance.TowerToWork = gameObject;
        UIManager.Instance.ShowUpdatePanel();
    }

    private void UpdateTower()
    {
        currentLevel++;

        UpdateParameters();
        CheckBulletList();

    }

    private void CheckBulletList()
    {
        int numberOfBulles = GetNumberOfActiveBullets();

        if(numberOfBulles>bullets.Count)
        {
            CreateABullet();
        }
    }

    public float GetBuildPrice()
    {
        return towerType.BuildPrice;
    }

    public float GetUpdatePrice()
    {
        return towerUpdate.GetUpdatePrice(towerClass, currentLevel);
    }
}
