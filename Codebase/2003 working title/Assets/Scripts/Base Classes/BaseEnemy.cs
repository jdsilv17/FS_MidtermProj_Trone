using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    #region Editable fields
    [SerializeField] protected int starthealth = 5, damage = 10;
    [SerializeField] protected float speed =5, rayCastLength=5;
    [SerializeField] protected ParticleSystem HitEffect = null;
    protected Animator anim = null;
    protected Rigidbody2D _rb = null;
    protected GameObject Player = null;
    #endregion
    public int health { get; protected set; }
    protected int playerLayer = 0, groundLayer = 0;
    protected bool onPlatform = false;
    protected bool _canTakeDamage = true;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        groundLayer = LayerMask.GetMask("Platform");
        playerLayer = LayerMask.GetMask("Player");
        health = starthealth;
        onPlatform = false;
        _canTakeDamage = true;
    }
    protected void Die()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    protected void TurnOnY() // Should really just use transform.Rotate() at this point, but I'll leave this for now
    {
        if (transform.right == Vector3.right)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
    }


    virtual public void TakeDamage(int _damage, Vector3 _damageSource)
    {
        if(_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                HitEffect.Play();
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                Die();
            }
        }
    }
    virtual public void TakeDamage(int _damage)
    {
        if(_canTakeDamage)
        {
            health -= _damage;
            if (_damage > 0)
            {
                HitEffect.Play();
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                Die();
            }
        }
    }
    virtual protected void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            _other.gameObject.GetComponent<PlayerController2D>().TakeDamage(damage);
        }
        if (_other.gameObject.tag == "MovingPlatform")
        {
            onPlatform = true;
        }
    }
}
