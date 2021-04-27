using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSelector : MonoBehaviour
{
    [SerializeField] AudioManager _AudioManager = null;
    [SerializeField] string SoundName = null;
    //[SerializeField] AudioMixerGroup master = null;
    private void Start()
    {
        PlaySound();
    }
    public void PlaySound()
    {
        _AudioManager.Play(SoundName);
    }
}
