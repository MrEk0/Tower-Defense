using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;
    [SerializeField] TextMeshProUGUI priceText;

    float range;
    float damage;
    float shootInterval;
    float buildPrice;
    GameObject bulletPrefab;

    float timeSinceLastShot=Mathf.Infinity;
    Transform target;

    private void Awake()
    {
        range = towerType.Range;
        damage = towerType.Damage;
        shootInterval = towerType.ShootInterval;
        buildPrice = towerType.BuildPrice;

        bulletPrefab = towerType.Bullet;
        priceText.text = buildPrice.ToString();
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
            //SmoothRotation(target);
            if (Vector2.Distance(transform.position, target.transform.position) > range)
            {
                Debug.Log("Find");
                FindNewTarget();
            }
            else
            {
                Vector2 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
                //transform.rotation=SmoothRotation(target);
                Shoot();
            }
        }
        //SmoothRotation(target);
    }

    private void FindNewTarget()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.zero);
        if (hit)
        {
            if (hit.collider.GetComponent<Enemy>() != null)
            {
                target = hit.transform;
            }
        }
    }

    private void Shoot()
    {
        if (timeSinceLastShot > shootInterval)
        {
            GameObject bullet= Instantiate(bulletPrefab, transform.position, transform.rotation, transform);
            bullet.GetComponent<Bullet>().target = target;
            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private Quaternion SmoothRotation(Transform target)
    {
        if (transform.position == target.position)
            return transform.rotation;

        Quaternion currentRotation = transform.rotation;

        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90);

        return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
