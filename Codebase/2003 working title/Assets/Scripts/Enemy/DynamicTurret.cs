using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTurret : BaseEnemy
{

    [SerializeField]
    float FireRate = 3f, burstTime = 0.5f;
    [SerializeField]
    GameObject Projectile = null;

    float timeSinceFire;
    private IEnumerator coroutine;
    Vector3 move_shot;
    Vector2 target = Vector2.zero;
    // Start is called before the first frame update
    [SerializeField] AudioManager SFX = null;
    override protected void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
        timeSinceFire = FireRate;
        move_shot = transform.position + new Vector3(0, 0.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) <= 10f)
        {
            target = Player.transform.position;
        }
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, playerLayer))
        {
            if (transform.position.x < target.x && transform.eulerAngles != Vector3.zero)
            {
                transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
            }
            else if (transform.position.x > target.x && transform.eulerAngles == Vector3.zero)
            {
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
            }
        }
        else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), rayCastLength, playerLayer))
        {
            if (transform.position.x < target.x && transform.eulerAngles != Vector3.zero)
            {
                transform.eulerAngles = transform.eulerAngles - new Vector3(0, 180, 0);
            }
            else if (transform.position.x > target.x && transform.eulerAngles == Vector3.zero)
            {
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
            }
        }
            if (onPlatform)
            {
                move_shot = transform.position + new Vector3(0.1f, 0.22f, 0);
            }
            timeSinceFire -= Time.deltaTime;
            if (timeSinceFire - Time.deltaTime <= 0)
            {
                coroutine = WaitAndFire(burstTime);
                StartCoroutine(coroutine);
                timeSinceFire = FireRate;
            }
        
    }

    void SpawnProjectile()
    {
        SFX.Play("DT_Shoot");
        GameObject shot = Instantiate(Projectile, move_shot, transform.rotation);
        shot.GetComponent<ProjectileController>().Init();
        shot.GetComponent<ProjectileController>().InitAnimation();
    }

    private IEnumerator WaitAndFire(float burstTime)
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(burstTime);
        }
    }
    override public void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if (_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                SFX.Play("DT_Damaged");
                HitEffect.Play();
            }
            if (health <= 0)
            {
                SFX.Play("Enemy_Death");
                Die();
            }
        }
    }
    public int RemainingHealth() { return health; }

}
