using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;

    float range;
    float damage;
    float shootInterval;
    GameObject bullet;

    float timeSinceLastShot=Mathf.Infinity;

    private void Awake()
    {
        range = towerType.Range;
        damage = towerType.Damage;
        shootInterval = towerType.ShootInterval;

        bullet = towerType.Bullet;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSinceLastShot>shootInterval)
        {
            Instantiate(bullet, transform.position, Quaternion.identity, transform);
            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
