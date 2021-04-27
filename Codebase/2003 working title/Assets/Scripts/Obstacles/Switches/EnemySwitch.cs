using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitch : BaseSwitch
{
    bool fighting = false, complete = false;
    [SerializeField] List<EnemyHandle> enemies = null;
    private void Start()
    {
        DisableObjects();
        foreach(EnemyHandle enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
    public override void Activate()
    {
        if (!complete)
        {

            //FindObjectOfType<AudioManager>().Play("PlayerDamaged");
            BeginFight();
        }
    }
    void BeginFight()
    {
        foreach(EnemyHandle enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        fighting = true;
        EnableObjects();
        FindObjectOfType<PlayerController2D>().UpdateCollisions();
    }
    void FightOver()
    {
        fighting = false;
        complete = true;
        DisableObjects();
        GetComponent<Collider2D>().enabled = false;
        //FindObjectOfType<AudioManager>().Play("EnemyFire");
    }
    private void Update()
    {
        if (fighting)
        {
            if(GetComponentsInChildren<EnemyHandle>().Length == 0)
            {
                FightOver();
            }
        }
    }
}
