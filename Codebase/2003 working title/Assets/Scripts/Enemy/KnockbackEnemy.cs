using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackEnemy : BaseEnemy
{

    #region Editable fields
    
    [SerializeField] float chargeSpeed = 2, chaseTime = 5f, baseChaseTime = 5f, thrust = 250f;
    [SerializeField] AudioManager SFX = null;
    #endregion

    bool Chasing = false;
    Vector2 target = Vector2.zero;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Chasing)
        {
            anim.SetBool("awaken", true);
            Invoke("Chase", 1f);
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

    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                HitEffect.Play();
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
            }
        }
    }
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SFX.Play("KB_Push");
            collision.GetComponent<Rigidbody2D>().velocity = (new Vector2(0, thrust));
            collision.GetComponent<PlayerController2D>().TakeDamage(damage);
        }
    }
}
