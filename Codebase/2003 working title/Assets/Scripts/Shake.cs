using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator _camAnimator;

    public void AttackShake()
    {
        _camAnimator.SetTrigger("AttackShake");
    }
}
