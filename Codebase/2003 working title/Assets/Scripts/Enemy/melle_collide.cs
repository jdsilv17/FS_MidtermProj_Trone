using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class melle_collide : MonoBehaviour
{

    protected bool startLife = false;
    Animator anim = null;
    public float LifeTime = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (startLife)
        {
            if (LifeTime > 0)
            {
                LifeTime -= Time.deltaTime;
            }
            else
                Kill();
        }
    }


    public void Init()
    {
        anim = GetComponent<Animator>();
    }
    public void InitAnimation()
    {
        anim.SetTrigger("Start");
        startLife = true;
    }
    public void Kill()
    {
        anim.SetTrigger("Kill");
    }
    void Remove()
    {
        Destroy(gameObject);
    }
}
