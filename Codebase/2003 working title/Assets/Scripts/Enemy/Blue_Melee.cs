using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Melee : BaseEnemy
{
    #region Editable fields
    [SerializeField]
    Transform dest = null;
    [SerializeField]
    GameObject collide = null;
    [SerializeField]
    Vector2 _knockback = Vector2.zero;
    [SerializeField] AudioManager BM = null;

    #endregion

    #region private
    Vector3 Target = Vector2.zero;
    Vector3 Home = Vector2.zero;
    #endregion


    override protected void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody2D>();
        Target = dest.position;
        Home = transform.position;
        anim = GetComponent<Animator>();
        //Marker.transform.position = Target;
        anim.Play("Idle_Melee");
    }

    void Update()
    {
        //Marker.transform.position = Target;
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down + (Vector2.right * .7f)), 5f, groundLayer) && !onPlatform)
        {
            //if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 0.01f, groundLayer))
            //{
            //    Turn();
            //    Home = transform.position;
            //}
            Move();
        }

    }
    void SpawnEffect()
    {
        GameObject _effect = (GameObject)Instantiate(collide, transform.position, transform.rotation, transform);
        _effect.GetComponent<melle_collide>().Init();
        _effect.GetComponent<melle_collide>().InitAnimation();
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

        //If enemy falls off original platform it will teleport to it's intended destination
        if (Mathf.Abs(Target.y - transform.position.y) >= 2f)
        {
            transform.position = Target;
        }
        transform.position = Vector2.MoveTowards(transform.position, Target, step);
    }


    public int RemainingHealth() { return health; }

    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if(_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                HitEffect.Play();
                Knockback(_knockback, _damageSource);
                BM.Play("BM_Damaged");
            }
            if (health <= 0)
            {
                BM.Play("Enemy_Death");
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
            }
        }
    }

    void Remove()
    {
        Destroy(gameObject);
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
        if (_other.CompareTag("Player"))
        {
            SpawnEffect();
            BM.Play("BM_Attack");
            _other.GetComponent<PlayerController2D>().TakeDamage(damage);
        }
        if (_other.gameObject.tag == "MovingPlatform")
        {
            onPlatform = true;
        }
    }
}
