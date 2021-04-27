using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surprise : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<ParticleSystem>().Play();
        FindObjectOfType<AudioManager>().Play("Horray");
    }
}
