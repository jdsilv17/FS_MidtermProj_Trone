using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : BaseEnemy
{

    [SerializeField]
    float FireRate = 3f, burstTime = 0.5f;
    [SerializeField]
    GameObject Projectile = null;
    [SerializeField] AudioManager SFX = null;

    float timeSinceFire = 0;
    private IEnumerator coroutine = null;
    Vector3 move_shot = Vector3.zero;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        timeSinceFire = FireRate;
        move_shot = transform.position + new Vector3(0.1f, 0.22f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), rayCastLength, groundLayer))
        {
            TurnOnY();
        }
        else
        {
            if(onPlatform)
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
    }

    void SpawnProjectile()
    {
        SFX.Play("BT_Shoot");
        GameObject shot = Instantiate(Projectile, move_shot, transform.rotation, transform);
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
        if(_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                SFX.Play("BT_Damaged");
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
