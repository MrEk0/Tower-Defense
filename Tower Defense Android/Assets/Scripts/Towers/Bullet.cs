using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    private float speed;
    private float damage;
    private float lifeTime;
    private float timeSinceStart=0f;

    private Transform target;
    private Rigidbody2D rb;
    private Transform myTransform;
    private GameObject explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();

        explosion = Instantiate(explosionPrefab, myTransform.position, Quaternion.identity);
        explosion.SetActive(false);
    }

    public void SetParameters(Transform target, float speed, float damage, float lifeTime)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;

        if (target.gameObject.activeInHierarchy == false || timeSinceStart>lifeTime)
        {
            ExplodeBullet();
        }
        else
        {
            BulletMovement();
        }
    }

    private void ExplodeBullet()
    {
        explosion.transform.position = myTransform.position;
        explosion.SetActive(true);
        timeSinceStart = 0f;
        gameObject.SetActive(false);
    }

    private void BulletMovement()
    {
        Vector2 dir = target.position - myTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        rb.velocity = myTransform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            timeSinceStart = 0f;
            gameObject.SetActive(false);
        }
    }
}
