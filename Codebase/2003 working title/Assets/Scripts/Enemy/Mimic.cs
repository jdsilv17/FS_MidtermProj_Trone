using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : BaseEnemy
{
    [SerializeField] float chargeSpeed = 2, chaseTime = 5f, baseChaseTime = 5f;
    [SerializeField] Vector2 _knockback = Vector2.zero, _hitBox = Vector2.zero;
    //[SerializeField] Transform attackPoint = null;
    [SerializeField] AudioManager SFX = null;


    bool Chasing = false;
    bool _transform = false;
    Vector2 target = Vector2.zero;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
        anim.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(_transform)
        {
            if (Chasing)
            {
                Chase();
            }
            else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, playerLayer))
            {
                Chasing = true;
            }
            else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), rayCastLength, playerLayer))
            {
                Chasing = true;
            }
        }
    }


    void Chase()
    {
        if (transform.position.x < target.x && transform.eulerAngles != Vector3.zero)
        {
            transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
        }
        else if (transform.position.x > target.x && transform.eulerAngles == Vector3.zero)
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
        }
        chaseTime -= Time.deltaTime;
        if (Vector2.Distance(transform.position, Player.transform.position) <= 10f && chaseTime > 0)
        {
            target = Player.transform.position;
        }
        else
        {
            chaseTime -= Time.deltaTime;
        }
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down + (Vector2.right * .7f)), 5f, groundLayer))
        {
            Move();
        }
        if (chaseTime <= 0f)
        {
            Chasing = false;
            chaseTime = baseChaseTime;
        }
    }
    void Move()
    {
        target = new Vector2(target.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * chargeSpeed);
    }

    private void Knockback(Vector2 knockbackPower, Vector2 contactPos)
    {
        _rb.velocity = Vector2.zero;


        if (contactPos.x > transform.position.x)
        {
            _rb.AddForce(new Vector2(-knockbackPower.x, knockbackPower.y));
        }
        else if (contactPos.x < transform.position.x)
        {
            _rb.AddForce(new Vector2(knockbackPower.x, knockbackPower.y));
        }
    }

    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                HitEffect.Play();
                SFX.Play("MM_Damaged");
                Knockback(_knockback, _damageSource);
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
                SFX.Play("Enemy_Death");
            }
        }
    }

    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(_transform)
        {
            if (collision.gameObject.tag == "Player")
            {
                anim.SetTrigger("Attack");
                SFX.Play("MM_Attack");
                collision.GetComponent<PlayerController2D>().TakeDamage(damage);
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player")
            {
                anim.SetBool("triggered", true);
                SFX.Play("MM_Transform");
                _transform = true;
                gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }
}
