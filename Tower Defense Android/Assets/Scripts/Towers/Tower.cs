using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Linq;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;

    float range;
    float shootInterval;
    float timeSinceLastShot = Mathf.Infinity;

    LayerMask enemyMask;    
    Transform lastWayPoint;
    GameObject bulletPrefab;   
    Transform target;
    Transform myTransform;
    List<GameObject> bullets;

    //public TowerSettings TowerType => towerType;

    private void Awake()
    {
        range = towerType.Range;
        shootInterval = towerType.ShootInterval;
        enemyMask = towerType.EnemyMask;
        //castlePoint = towerType.CastlePoint;
        bulletPrefab = towerType.Bullet;

        myTransform = GetComponent<Transform>();

        CreateListOfBullets();
    }

    private void Start()
    {
        lastWayPoint = GameManager.GetLastPathPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void CreateListOfBullets()
    {
        bullets = new List<GameObject>();
        int numberOfBullets = GetNumberOfActiveBullets();
        
        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, myTransform.position, Quaternion.identity, myTransform);
            bullet.GetComponent<Bullet>().SetParameters(towerType.BulletSpeed, towerType.Damage);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }

    private int GetNumberOfActiveBullets()
    {
        float timeToGetRange = range / towerType.BulletSpeed;
        return Mathf.CeilToInt(timeToGetRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Rotate()
    {
        Vector2 dir = target.position - myTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    private void FindNewTarget()
    {
        float distanceToEnemy = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, range, enemyMask);
        for(int i=0; i<colliders.Length; i++)
        {
            float currentDistance = Vector2.Distance(lastWayPoint.position, colliders[i].transform.position);
            if (currentDistance < distanceToEnemy)
            {
                distanceToEnemy = currentDistance;
                target = colliders[i].transform;
            }
        }
    }

    private void Shoot()
    {
        if (timeSinceLastShot > shootInterval)
        {
            GameObject bullet = bullets.First(b => !b.activeInHierarchy);
            bullet.transform.position = myTransform.position;
            bullet.transform.rotation = myTransform.rotation;
            bullet.GetComponent<Bullet>().target = target;
            bullet.SetActive(true);

            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    //private Quaternion SmoothRotation(Transform target)
    //{
    //    if (myTransform.position == target.position)
    //        return myTransform.rotation;

    //    Quaternion currentRotation = myTransform.rotation;

    //    Vector2 dir = target.position - myTransform.position;
    //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);

    //    return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10f);
    //}

    private void OnMouseDown()
    {   
        UIManager.Instance.ShowSellPanel();
        UIManager.Instance.towerToSell = gameObject;
    }

    public float GetBuildPrice()
    {
        return towerType.BuildPrice;
    }
}
