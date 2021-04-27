using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    protected bool triggered = false;
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            GetComponent<AudioManager>().Play("VoiceLine");
            triggered = true;
        }
    }
}
