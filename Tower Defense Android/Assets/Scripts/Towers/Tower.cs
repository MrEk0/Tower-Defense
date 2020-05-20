using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;
    [SerializeField] TowerClass towerClass;
    [SerializeField] TowerUpdate towerUpdate;

    private int currentLevel = 1;
    private float range;
    private float shotInterval;
    private float damage;
    private float bulletLifeTime;
    private float timeSinceLastShot = Mathf.Infinity;

    private LayerMask enemyMask;
    private Vector3 lastPointPosition;
    private GameObject bulletPrefab;
    private Transform target;
    private Transform myTransform;
    private List<GameObject> bullets;
    private Collider2D enemy;

    private void Awake()
    {
        UpdateParameters();
        enemyMask = towerType.EnemyMask;
        bulletPrefab = towerType.Bullet;

        myTransform = GetComponent<Transform>();
        lastPointPosition = GameManager.GetLastPathPoint().position;

        CreateListOfBullets();
    }

    void Update()
    {
        if (GameManager.isGamePaused || GameManager.isGameOver)
            return;

        if (target == null || !target.gameObject.activeInHierarchy
            || Vector2.Distance(myTransform.position, target.transform.position) > range)
        {
            FindNewTarget();
        }
        else
        {
            Rotate();
            Shoot();
        }

        timeSinceLastShot += Time.deltaTime;
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
        bulletLifeTime = range / towerType.BulletSpeed;
        return Mathf.CeilToInt(bulletLifeTime / shotInterval);
    }

    private void FindNewTarget()
    {
        enemy = Physics2D.OverlapCircle(myTransform.position, range, enemyMask);//to avoid constanct array creation
        if (!enemy)
            return;

        float distanceToEnemy = Mathf.Infinity;
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(myTransform.position, range, enemyMask);

        if (enemies.Length == 0)
            return;

        for (int i = 0; i < enemies.Length; i++)
        {
            float currentDistance = Vector2.Distance(lastPointPosition, enemies[i].transform.position);

            if (currentDistance < distanceToEnemy)
            {
                distanceToEnemy = currentDistance;
                target = enemies[i].transform;
            }
        }
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
    }

    private void SetUpBullet()
    {
        GameObject bullet = bullets.First(b => !b.activeInHierarchy);
        bullet.GetComponent<Bullet>().SetParameters(target, towerType.BulletSpeed, damage, bulletLifeTime);
        bullet.transform.position = myTransform.position;
        bullet.transform.rotation = myTransform.rotation;
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (RectTransformExtensions.IsPointerOverUIObject())
            return;

        UIManager.Instance.TowerToWork = this;
        UIManager.Instance.ShowUpdatePanel();
    }

    public void UpdateTower()
    {
        currentLevel++;

        UpdateParameters();
        CheckBulletList();
    }

    private void CheckBulletList()
    {
        int numberOfBullets = GetNumberOfActiveBullets();

        if(numberOfBullets>bullets.Count)
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
