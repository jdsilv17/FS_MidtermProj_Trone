using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestingChild : BaseEnemy
{
    [SerializeField]
    float chargeSpeed = 2, chaseTime = 5f, baseChaseTime = 5f;
    [SerializeField]
    Vector2 _knockback = Vector2.zero;
    [SerializeField]
    GameObject _child = null, _child2 = null;
    [SerializeField] AudioManager SFX = null;

    bool Chasing = false;
    Vector2 target = Vector2.zero;
    Vector3 moveChild = Vector3.zero;
    Vector3 moveChild2 = Vector3.zero;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
        anim = GetComponent<Animator>();
        moveChild = transform.position + new Vector3(0.5f, 0, 0);
        moveChild2 = transform.position + new Vector3(0.75f, 0, 0);
        Player.SendMessage("UpdateCollisions");
    }

    // Update is called once per frame
    void Update()
    {
        if (Chasing && _canTakeDamage)
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
        if (Chasing && chaseTime > 0)
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

    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                SFX.Play("ND_Damaged");
                HitEffect.Play();
                Knockback(_knockback, _damageSource);
            }
            if (health <= 0)
            {
                SFX.Play("ND_Break1");
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
                Invoke("split", 1f);
            }
        }
    }

    private void split()
    {
        Instantiate(_child, moveChild, transform.rotation);
        Instantiate(_child2, moveChild2, transform.rotation);
        Die();
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
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SFX.Play("ND_Attack");
            collision.GetComponent<PlayerController2D>().TakeDamage(10);
        }
    }
}
