using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSwitch : BaseSwitch
{
    //How long you want objects to be turned on or off
    [SerializeField] float duration = 0;
    //If you want to start with objects on set to false.
    [SerializeField] bool toggleOn = false;
    [SerializeField] AudioManager sfx = null;
    bool finished = false, triggered = false;
    public override void Activate()
    {
        finished = false;
        if (!triggered)
        {
            triggered = true;
            if (toggleOn)
            {
                StartCoroutine(ToggleOn());
            }
            else
            {
                StartCoroutine(ToggleOff());
            }
        }
    }
    IEnumerator Timer(float Time)
    {
        if (!finished)
        {
            sfx.Play("Tick");
            yield return new WaitForSeconds(Time);
            if(Time < .1f)
            {
                Time = .1f;
            }
            else
            {
                Time /= 2;
            }
            StartCoroutine(Timer(Time));
        }
    }
    IEnumerator ToggleOn()
    {
        EnableObjects();
        StartCoroutine(Timer(duration / 4));
        yield return new WaitForSeconds(duration);
        finished = true;
        triggered = false;
        DisableObjects();
    }
    IEnumerator ToggleOff()
    {
        DisableObjects();
        StartCoroutine(Timer(duration / 4));
        yield return new WaitForSeconds(duration);
        finished = true;
        triggered = false;
        EnableObjects();
    }
}
