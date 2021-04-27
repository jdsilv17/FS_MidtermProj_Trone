using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EnemyHandle))]
public class WallWalker : BaseEnemy
{

    [SerializeField]
    Transform dest = null;
    [SerializeField]
    float FireRate = 1.5f, wait = 0.5f;
    [SerializeField]
    GameObject Projectile = null;
    [SerializeField]
    Quaternion angle = new Quaternion(0, 0, 0, 0);
    [SerializeField] AudioManager SFX = null;


    Vector3 Target = Vector2.zero;
    Vector3 Home = Vector2.zero;
    bool shooting = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Target = dest.position;
        Home = transform.position;
        anim = GetComponent<Animator>();
        anim.Play("Walking");
    }

    // Update is called once per frame
    void Update()
    {
        FireRate -= Time.deltaTime;
        if (FireRate <= 0)
        {
            shooting = true;
            SpawnProjectile();
        }
        if (shooting)
        {
            StartCoroutine(StopAndShoot(wait));
            FireRate = 1.5f;
        }
        Move();

    }

    void SpawnProjectile()
    {
        SFX.Play("WW_Shoot");
        GameObject shot = Instantiate(Projectile, transform.position, (transform.rotation * angle));
        shot.GetComponent<ProjectileController>().Init();
        shot.GetComponent<ProjectileController>().InitAnimation();
    }

    private void Move()
    {
        if (!shooting)
        {
            float step = speed * Time.deltaTime;

            ///////////////////////////////////////////////////////
            // If the enemy reached it's target position,
            // turn around, and reset target
            ///////////////////////////////////////////////////////
            if (Vector3.Distance(transform.position, Target) /*Mathf.Abs(transform.position.y - Target.y)*/ <= 0.01f)
            {
                //TurnOnY();
                transform.Rotate(Vector3.up, 180f);
                if (Target == Home)
                {
                    Target = dest.position;
                    //transform.eulerAngles = new Vector3(0, 0, 90);
                }
                else
                {
                    Target = Home;
                    //transform.eulerAngles = new Vector3(180, 0, 90);
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, Target, step);
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
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                anim.SetBool("Dying", true);
            }
        }
    }
    private IEnumerator StopAndShoot(float wait)
    {
        yield return new WaitForSeconds(wait);
        shooting = false;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SFX.Play("WW_Attack");
            collision.gameObject.GetComponent<PlayerController2D>().TakeDamage(10);
        }
    }
}
