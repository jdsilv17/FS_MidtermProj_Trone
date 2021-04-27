using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] _sounds = null;
    public AudioMixer _AudioMixer = null;
    [SerializeField] int SoundMaxDistance = 7;
    [SerializeField] int SoundMinDistance = 7;
    [Range(0.0f, 1.0f)] public float BaseVolume = 1.0f;
    [SerializeField] bool sound3d = false;

    void Awake()
    {
        
        foreach (Sound sound in _sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound._clip;
            sound.source.volume = (BaseVolume + sound._volume)/2;
            sound.source.pitch = sound._pitch;
            sound.source.loop = sound._loop;
            sound.source.outputAudioMixerGroup = sound._group;
            sound.source.playOnAwake = false;
            if (sound3d == true)
            {
                sound.source.spatialBlend = 1;
                sound.source.rolloffMode = AudioRolloffMode.Linear;
                sound.source.maxDistance = SoundMaxDistance;
                sound.source.minDistance = SoundMinDistance;
            }
        }
        _AudioMixer.SetFloat("MasterVolume", Mathf.Log10 (PlayerPrefs.GetFloat("MasterVolume")) * 20);
        _AudioMixer.SetFloat("MusicVolume", Mathf.Log10 (PlayerPrefs.GetFloat("MusicVolume")) * 20);
        _AudioMixer.SetFloat("SFXVolume", Mathf.Log10 (PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }

    private int SongSearch (string name)
    {
        for(int loop = 0; loop < _sounds.Length; loop++)
        {
            if (_sounds[loop]._name == name)
            {
                return loop;
            }
        }
        return -1;
    }

    public void Play (string name)
    {
        Sound sound;
        int soundNumber = SongSearch(name);
        sound = _sounds[soundNumber];
        sound.source.Play();
    }

    public void Stop (string name)
    {
        Sound sound;
        int soundNumber = SongSearch(name);
        sound = _sounds[soundNumber];
        sound.source.Stop();
    }

    public void Pause (string name)
    {
        Sound sound;
        int soundNumber = SongSearch(name);
        sound = _sounds[soundNumber];
        sound.source.Pause();
    }
    public void Unpause (string name)
    {
        Sound sound;
        int soundNumber = SongSearch(name);
        sound = _sounds[soundNumber];
        sound.source.UnPause();
    }
    public void StopAll ()
    {
        foreach(Sound sound in _sounds)
        {
            sound.source.Stop();
        }
    }
    public void PauseAll ()
    {
        foreach(Sound sound in _sounds)
        {
            sound.source.Pause();
        }
    }
    public void UnpauseAll ()
    {
        foreach(Sound sound in _sounds)
        {
            sound.source.UnPause();
        }
    }
}
