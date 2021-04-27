using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAudioTrigger : AudioTrigger
{
    [SerializeField] string EventName = null;
    private void Start()
    {
        if(PlayerPrefs.GetInt(EventName) == 1)
        {
            triggered = true;
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt(EventName, 1);
        base.OnTriggerEnter2D(collision);
    }
}
