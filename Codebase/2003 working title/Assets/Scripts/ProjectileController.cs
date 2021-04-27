using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public bool PlayerOwned = false;
    public bool PassThroughEnemies = false;
    public bool PassThroughWalls = false;
    public int Damage = 10;
    public float LifeTime = 1.2f;
    public int speed = 20;

    Vector2 StartVelocity;
    protected bool startLife = true;
    Animator anim;
    Rigidbody2D body = null;
    int groundLayer = 0;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        Vector2 direction = transform.right;
        StartVelocity = direction * speed;
        groundLayer = LayerMask.GetMask("Platform");
    }
    void Update()
    {
        if (startLife)
        {
            if (LifeTime > 0)
            {
                LifeTime -= Time.deltaTime;
            }
            else
                Kill();
        }
    }

    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = -newScale.x;
        transform.localScale = newScale;
        StartVelocity.x = -StartVelocity.x;
    }

    public void Init()
    {
        //anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        if (body)
        {
            if (PlayerOwned)
            {
                body.velocity = StartVelocity * transform.parent.transform.right;
                transform.SetParent(null);
            }
            else
                body.velocity = StartVelocity;
        }

    }
    public void InitAnimation()
    {
        anim.SetTrigger("Start");
        startLife = true;
    }

    public void Kill()
    {
        anim.SetTrigger("Kill");
        if (body)
            body.velocity = Vector3.zero;
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!PlayerOwned)
        {
            if (!collision.otherCollider.CompareTag("Player"))
            {
                Kill();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (startLife)
        {
            if (PlayerOwned)
            {
                if (_other.CompareTag("Enemy"))
                {
                    _other.gameObject.GetComponent<EnemyHandle>().TakeDamage(Damage, transform.position);
                    Kill();
                }
                
            }
            else
            {
                if (_other.CompareTag("Player"))
                {
                    _other.gameObject.GetComponent<PlayerController2D>().TakeDamage(Damage);
                    Kill();
                }
            }
        }
    }
}
