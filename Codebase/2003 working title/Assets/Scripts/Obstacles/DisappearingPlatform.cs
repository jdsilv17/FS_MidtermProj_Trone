using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField]
    float timer = 1f;
    [SerializeField] AudioManager SFX = null;

    Animator anim = null;
    SpriteRenderer _sr = null;
    Collider2D[] col = null;
    GameObject Player = null;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        col = GetComponents<Collider2D>();
        Player = FindObjectOfType<PlayerController2D>().gameObject;
    }
    private void Update()
    {
        if(!_sr.enabled)
        {
            timer -= Time.deltaTime;
            if (Vector2.Distance(transform.position, Player.transform.position) <= 1f)
            {
                timer = 1f;
            }
            else
            {
                if(timer <= 0)
                {
                    _sr.enabled = true;
                    SFX.Play("DP_Appear");
                    foreach (Collider2D item in col)
                    {
                        item.enabled = true;
                    }
                    timer = 1f;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            anim.SetBool("blink", true);
            StartCoroutine(Disappear());
        }
    }
    private IEnumerator Disappear()
    {
        SFX.Play("DP_Disappear");
        yield return new WaitForSeconds(1f);
        anim.SetBool("blink", false);
        _sr.enabled = false;
        foreach (Collider2D item in col)
        {
            item.enabled = false;
        }
    }
}
