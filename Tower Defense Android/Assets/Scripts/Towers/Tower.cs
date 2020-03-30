using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

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

    //public TowerSettings TowerType => towerType;

    private void Awake()
    {
        range = towerType.Range;
        shootInterval = towerType.ShootInterval;
        enemyMask = towerType.EnemyMask;
        //castlePoint = towerType.CastlePoint;
        bulletPrefab = towerType.Bullet;

        myTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        lastWayPoint = GameManager.GetLastPathPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindNewTarget();
        }
        else
        {
            //if (Vector2.Distance(myTransform.position, target.transform.position) > range)
            if(Vector2.SqrMagnitude(target.transform.position-myTransform.position)>range)
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

    private void Rotate()
    {
        Vector2 dir = target.position - myTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    private void FindNewTarget()
    {
        float distance = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(myTransform.position, range, enemyMask);
        foreach (Collider2D enemy in colliders)
        {
            if (Vector2.Distance(lastWayPoint.position, enemy.transform.position) < distance)
            {
                distance = Vector2.Distance(lastWayPoint.position, enemy.transform.position);
                target = enemy.transform;
            }
        }
    }

    private void Shoot()
    {
        if (timeSinceLastShot > shootInterval)
        {
            GameObject bullet = Instantiate(bulletPrefab, myTransform.position, myTransform.rotation, myTransform);
            bullet.GetComponent<Bullet>().target = target;
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
        //UIManager uIManager = FindObjectOfType<UIManager>();
        
        UIManager.Instance.ShowSellPanel();
        UIManager.Instance.towerToSell = gameObject;
    }

    public float GetBuildPrice()
    {
        return towerType.BuildPrice;
    }
}
