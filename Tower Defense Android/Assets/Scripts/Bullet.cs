using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;

    float speed;
    float damage;
    Rigidbody2D rb;
    Transform myTransform;

    public Transform target { private get; set; }
    private void Awake()
    {
        speed = towerType.BulletSpeed;
        damage = towerType.Damage;

        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector2 dir = target.position - myTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            myTransform.rotation = Quaternion.Euler(0f, 0f, angle-90);

            rb.velocity = myTransform.up * speed;
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
