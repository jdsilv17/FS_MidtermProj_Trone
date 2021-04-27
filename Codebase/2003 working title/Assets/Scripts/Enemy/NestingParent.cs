using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestingParent : BaseEnemy
{

    [SerializeField]
    Vector2 _knockback = Vector2.zero;
    [SerializeField]
    Transform dest = null;
    [SerializeField]
    GameObject _child = null;
    [SerializeField]
    GameObject _child2 = null;
    [SerializeField] AudioManager SFX = null;

    Vector3 Target = Vector2.zero;
    Vector3 Home = Vector2.zero;
    Vector3 moveChild = Vector3.zero;
    Vector3 moveChild2 = Vector3.zero;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Target = dest.position;
        Home = transform.position;
        moveChild = transform.position + new Vector3(0.5f, 0, 0);
        moveChild2 = transform.position + new Vector3(0.75f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(_canTakeDamage)
        {
            Move();
        }
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;


        ///////////////////////////////////////////////////////
        // If the enemy reached it's target position,
        // turn around, and reset target
        ///////////////////////////////////////////////////////
        if (Mathf.Abs(transform.position.x - Target.x) <= 0.01f)
        {
            TurnOnY();
            if (Target == Home)
            {
                Target = dest.position;
            }
            else
            {
                Target = Home;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, Target, step);
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

    override protected void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            SFX.Play("ND_Attack");
            _other.gameObject.GetComponent<PlayerController2D>().TakeDamage(10);
        }
    }
}
