using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] AudioManager SFX = null;
    [SerializeField] ParticleSystem Bubbles = null;

    private void Start()
    {
        Bubbles.Play();
        SFX.Play("AP_Bubbles");
    }

    private void OnTriggerEnter2D(Collider2D _player)
    {
        if (_player.gameObject.tag == "Player")
        {
            SFX.Play("AP_Damage");
            _player.gameObject.GetComponent<PlayerController2D>().GameOver();
        }
        else if(_player.gameObject.CompareTag("Enemy"))
        {
            SFX.Play("AP_Damage");
            _player.gameObject.GetComponent<EnemyHandle>().TakeDamage(50);
        }
    }
}
