using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioManager PS = null;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // this is where it finds  the player that collides with the upgrade
        if (collision.CompareTag("Player"))
        {
            PlayerController2D player = collision.GetComponent<PlayerController2D>();
            player.CurrentHealth = player.GetMaxHealth();
            PS.Play("P_Heal");
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ParticleSystem>().Play();
            Destroy(this, GetComponent<ParticleSystem>().main.duration);
        }
    }
}
