using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;
    //[SerializeField] TextMeshProUGUI priceText;
    //[SerializeField] GameObject sellPanelPrefab;
    //[SerializeField] LayerMask enemyMask;

    float range;
    float shootInterval;
    LayerMask enemyMask;
    float buildPrice;
    Transform castlePoint;
    GameObject bulletPrefab;

    float timeSinceLastShot=Mathf.Infinity;
    Transform target;

    public float BuildPrice  => buildPrice;

    private void Awake()
    {
        range = towerType.Range;
        shootInterval = towerType.ShootInterval;
        buildPrice = towerType.BuildPrice;
        enemyMask = towerType.EnemyMask;
        castlePoint = towerType.CastlePoint;

        bulletPrefab = towerType.Bullet;
        //priceText.text = buildPrice.ToString();
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
        float distance = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);
        foreach (Collider2D enemy in colliders)
        {
            if (Vector2.Distance(castlePoint.position, enemy.transform.position) < distance)
            {
                distance = Vector2.Distance(castlePoint.position, enemy.transform.position);
                target = enemy.transform;
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

    private void OnMouseDown()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;

        //Debug.Log("Click");
        //sellPanelPrefab.SetActive(true);

        //Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 roundMousePos = new Vector2(Mathf.RoundToInt(screenPoint.x), Mathf.RoundToInt(screenPoint.y));

        //Vector2 pointToMove = Camera.main.WorldToScreenPoint(roundMousePos);
        //sellPanelPrefab.GetComponent<RectTransform>().position = pointToMove;
        FindObjectOfType<UIManager>().ShowSellPanel();
        FindObjectOfType<UIManager>().towerToSell = gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
