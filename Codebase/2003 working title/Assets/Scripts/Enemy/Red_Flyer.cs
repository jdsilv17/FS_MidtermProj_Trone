using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Red_Flyer : BaseEnemy
{
    #region Editable fields
    [SerializeField]
    Transform dest = null;
    [SerializeField]
    float FireRate = 0;
    [SerializeField]
    GameObject Projectile = null;
    [SerializeField] AudioManager SFX = null;
    [SerializeField]
    bool flipped = false;
    #endregion

    #region private
    //float attackTimer = 0;
    private float _waitTime = 2.0f;
    Vector3 Target = Vector2.zero;
    Vector3 Home = Vector2.zero;
    float timeSinceFire = 0;
    #endregion


    override protected void Start()
    {
        base.Start();
        Target = dest.position;
        Home = transform.position;
        anim = GetComponent<Animator>();
        timeSinceFire = FireRate;
        _waitTime = (Vector2.Distance(transform.position, Target) / speed) + 2.0f;
        anim.Play("Idle_Flyer");
    }

    void Update()
    {
        #region raycast

        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, playerLayer))
        {
            timeSinceFire -= Time.deltaTime;
            if (timeSinceFire - Time.deltaTime <= 0)
            {
                timeSinceFire = FireRate;
                SpawnProjectile();
            }
        }
        else
        {
            Move();
        }
        #endregion
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;
        ///////////////////////////////////////////////////////
        // If the enemy reached it's target position,
        // turn around, and reset target and timer
        ///////////////////////////////////////////////////////
        if (Vector3.Distance(transform.position, Target)  <= 0.01f)
        {
            transform.Rotate(0, 180f, 0);
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


    void SpawnProjectile()
    {
        if(!flipped)
        {
            GameObject shot = Instantiate(Projectile, (transform.position - new Vector3(0, .12f, 0)), transform.rotation, transform);
            SFX.Play("RF_Shoot");
            shot.GetComponent<ProjectileController>().Init();
            shot.GetComponent<ProjectileController>().InitAnimation();
        }
        else
        {
            GameObject shot = Instantiate(Projectile, (transform.position - new Vector3(0, .12f, 0)), (transform.rotation * new Quaternion(0, 180, 0, 0)), transform);
            SFX.Play("RF_Shoot");
            shot.GetComponent<ProjectileController>().Init();
            shot.GetComponent<ProjectileController>().InitAnimation();
        }
    }

    public int RemainingHealth() { return health; }

    public override void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if(_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                SFX.Play("RF_Damaged");
                HitEffect.Play();
            }
            if (transform.right == Vector3.right && _damageSource.x < transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                if (Target == Home)
                {
                    Target = dest.position;
                }
                else
                {
                    Target = Home;
                }
            }
            else if (transform.right != Vector3.right && _damageSource.x > transform.position.x)
            {
                transform.eulerAngles = Vector3.zero;
                if (Target == Home)
                {
                    Target = dest.position;
                }
                else
                {
                    Target = Home;
                }
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
                SFX.Play("Enemy_Death");
            }
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
