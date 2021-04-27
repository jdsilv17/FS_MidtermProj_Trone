using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : BaseEnemy
{

    [SerializeField] float chargeSpeed = 2, chaseTime = 5f, baseChaseTime = 5f, radius = 1.5f, fuse = 0.3f;

    bool Chasing = false;
    bool exploding = false;
    Vector2 target = Vector2.zero;
    [SerializeField] AudioManager SFX = null;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
        anim.Play("Idle");
    }


    // Update is called once per frame
    void Update()
    {
        if (Chasing)
        {
            Chase();
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, playerLayer))
        {
            SFX.Play("KM_Alert");
            Chasing = true;
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), rayCastLength, playerLayer))
        {
            SFX.Play("KM_Alert");
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
        if(exploding)
        {
            fuse -= Time.deltaTime;
            if(fuse <= 0)
            {
                SFX.Play("Enemy_Death");
                anim.SetBool("explode", true);
                anim.SetBool("stop", false);
            }
        }
        if(!exploding)
        {
            Move();
        }
        if (chaseTime <= 0f || Vector2.Distance(transform.position, Player.transform.position) >= 12f)
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
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            exploding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            exploding = false;
            HitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            anim.SetBool("explode", false);
            anim.SetBool("stop", true);
        }
    }
    private void Explode()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        foreach (Collider2D p in player)
        {
            PlayerController2D _player = p.GetComponent<PlayerController2D>();
            if (_player != null)
            {
                SFX.Play("KM_Explode");
                _player.TakeDamage(damage);
                Invoke("Remove", 0.5f);
            }
        }
    }
    override public void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (_canTakeDamage)
        {
            health -= _damage;
            if (health <= 0)
            {
                SFX.Play("Enemy_Death");
                _canTakeDamage = false;
                exploding = false;
                Die();
            }
        }
    }
    void ExplodeEffect()
    {
        HitEffect.Play();
    }
    private void Remove()
    {
        Destroy(gameObject);
    }
}
