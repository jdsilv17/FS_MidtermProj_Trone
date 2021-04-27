using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwitch : BaseSwitch
{
    [SerializeField] bool turnOn = false;
    [SerializeField] AudioManager SFX = null;
    public override void Activate()
    {
        if (turnOn)
        {
            EnableObjects();
            SFX.Play("SW_Activate");
        }
        else
        {
            DisableObjects();
            SFX.Play("SW_Deactivate");
        }
    }
}
