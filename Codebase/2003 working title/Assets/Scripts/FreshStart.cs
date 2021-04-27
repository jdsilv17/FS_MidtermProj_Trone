using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshStart : MonoBehaviour
{
    public float MasterVolume = 0;
    public float MusicVolume = 0;
    public float SFXVolume = 0;
    public bool FullScreen = true;
    public float ResolutionWidth = 0;
    public float ResolutionHeight = 0;

    //This function will restart you back to the beginning of the game in terms of saved progression
    public void ClearPlayerPrefs()
    {
        SaveSettingPrefs();
        PlayerPrefs.DeleteAll();
        ReloadSaveSettingPrefs();
    }

    private void SaveSettingPrefs()
    {
        ResolutionWidth = PlayerPrefs.GetFloat("ResolutionWidth");
        ResolutionHeight = PlayerPrefs.GetFloat("ResolutionHeight");

        //Setup FullScreen
        FullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;

        // Setup Volume Mixer
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.2f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
    }
    private void ReloadSaveSettingPrefs()
    {
        PlayerPrefs.SetFloat("ResolutionWidth", ResolutionWidth);
        PlayerPrefs.SetFloat("ResolutionHeight", ResolutionHeight);

        if (FullScreen == true)
            PlayerPrefs.SetInt("FullScreen", 1);
        else
            PlayerPrefs.SetInt("FullScreen", 0);

        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        PlayerPrefs.Save();
    }
}
