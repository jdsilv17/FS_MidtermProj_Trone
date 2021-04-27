using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpikes : MonoBehaviour
{
    [SerializeField] AudioManager SFX = null;
    [SerializeField] float thrust = 0;
    [SerializeField] bool onGround = false, onRoof = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (collision.gameObject.tag == "Player")
        {
            SFX.Play("SP_Damage");
            collision.GetComponent<PlayerController2D>().TakeDamage(10);
            if(onGround)
            {
                collision.GetComponent<Rigidbody2D>().velocity = (new Vector2(0, thrust));
            }
            else if(onRoof)
            {
                collision.GetComponent<Rigidbody2D>().velocity = (new Vector2(0, -thrust));
            }
        }
    }
}
