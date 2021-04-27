using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teleporter : BaseEnemy
{
    [SerializeField]
    float timeSincePort = 0;
    [SerializeField]
    Transform[] Spots = null;
    [SerializeField] AudioManager SFX = null;


    float teleport = 0f;
    int size = 0;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        teleport = timeSincePort;
        size = Spots.Length;
    }

    // Update is called once per frame
    void Update()
    {
        teleport -= Time.deltaTime;
        if(teleport <= 0)
        {
            anim.SetBool("Teleporting", true);
        }
    }

    private void Teleport()
    {
        int number = Random.Range(1, size + 1);
        transform.position = Spots[(number - 1)].position;
        teleport = timeSincePort;
        SFX.Play("TP_Teleport");
        anim.SetBool("Teleporting", false);
    }


    public override void TakeDamage(int _damage, Vector3 _hitPosition)
    {
        health -= _damage;
        if(_canTakeDamage)
        {
            if (_damage > 0)
            {
                SFX.Play("TP_Damaged");
                HitEffect.Play();
                anim.SetBool("Teleporting", true);
            }
            if (health <= 0)
            {
                _canTakeDamage = false;
                SFX.Play("TP_Damaged");
                anim.SetBool("Dying", true);
            }
        }
    }
}
