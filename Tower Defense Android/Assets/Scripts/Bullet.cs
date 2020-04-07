using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    float damage;
    Rigidbody2D rb;
    Transform myTransform;

    public Transform target { private get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
    }

    public void SetParameters(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Vector2 dir = target.position - myTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            myTransform.rotation = Quaternion.Euler(0f, 0f, angle-90);

            rb.velocity = myTransform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
