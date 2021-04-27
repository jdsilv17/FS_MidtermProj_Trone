////////////////////////////////////////////////////////////////////////////////
// File name:		Settings Menu
//
// Purpose:		    Changing options in game for player prefrences
//
// Related Files:	N/A
//
// Author:			Amara Gitomer  / Steven Lee
//
// Created:			4/27/20        / 5/6/20
//
// Last Modified:	4/28/20        / 5/6/20    5/11/20
///////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] AudioMixer _AudioMixer = null;
    [SerializeField] Dropdown ResolutionDropdown = null;
    [SerializeField] List<Resolution> resolutions = new List<Resolution>();
    [SerializeField] Slider MasterSlider = null;
    [SerializeField] Slider MusicSlider = null;
    [SerializeField] Slider SFXSlider = null;
    public float MasterVolume = 0;
    public float MusicVolume = 0;
    public float SFXVolume = 0;
    public bool FullScreen = true;
    public float ResolutionWidth = 0;
    public float ResolutionHeight = 0;

    /// 
    /// gathering the resolution information for the resolution option in Settings to be able to pull from players personal computer
    /// Also sets the keeps the current resolution on the players computer as the defualt resolution
    ///
     void Start()
    {
        foreach(Dropdown.OptionData opt in ResolutionDropdown.options)
        {
            string[] strTemp = opt.text.Split('x');
            Resolution resTemp = new Resolution();
            resTemp.width = int.Parse(strTemp[0]);
            resTemp.height = int.Parse(strTemp[1]);
            resolutions.Add(resTemp);
        }

        resolutions.Add(Screen.currentResolution);
        string temp = Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
        List<string> temp2 = new List<string>();
        temp2.Add(temp);
        ResolutionDropdown.AddOptions(temp2);
        for(int i = 0; i < resolutions.Count-1;i++)
        {
            if (resolutions[i].width == resolutions[resolutions.Count-1].width && resolutions[i].height == resolutions[resolutions.Count - 1].height)
            {
                resolutions.RemoveAt(i);
                ResolutionDropdown.options.RemoveAt(i);
                break;
            }
        }
        ResolutionDropdown.value = ResolutionDropdown.options.Count;
        //List<string> listOfResolutions = new List<string>();

        //int CurrentResolutionIndex = 0;

        //for (int i = 0; i < resolutions.Length; i++)
        //{
        //    string option = resolutions[i].width + " x " + resolutions[i].height;
        //    listOfResolutions.Add(option);

        //    if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
        //    {
        //        CurrentResolutionIndex = i;
        //    }
        //}

        //ResolutionDropdown.AddOptions(listOfResolutions);
        //ResolutionDropdown.value = CurrentResolutionIndex;
        //ResolutionDropdown.RefreshShownValue();

        //Setup Resolution
        ResolutionWidth = PlayerPrefs.GetFloat("ResolutionWidth");
        ResolutionHeight = PlayerPrefs.GetFloat("ResolutionHeight");

        //Setup FullScreen
        FullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;

        // Setup Volume Mixer
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.2f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);

        MasterSlider.value = MasterVolume;
        MusicSlider.value = MusicVolume;
        SFXSlider.value = SFXVolume;

        _AudioMixer.SetFloat("MasterVolume", Mathf.Log10 (MasterVolume) * 20);
        _AudioMixer.SetFloat("MusicVolume", Mathf.Log10 (MusicVolume) * 20);
        _AudioMixer.SetFloat("SFXVolume", Mathf.Log10 (SFXVolume) * 20);
     }

    public void SetResolution (int resolutionindex)
    {
        Resolution resolutionSetting = resolutions[resolutionindex];
        Screen.SetResolution(resolutionSetting.width, resolutionSetting.height, Screen.fullScreen);
        PlayerPrefs.SetFloat("ResolutionWidth", resolutionSetting.width);
        PlayerPrefs.SetFloat("ResolutionHeight", resolutionSetting.height);
        PlayerPrefs.Save();
    }

    /// 
    /// Setting Master Volume of Audio to what the player wants in the settings menu
    /// 
    public void SetMasterLevel(float volume)
    {
        _AudioMixer.SetFloat("MasterVolume", Mathf.Log10 (volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetMusicLevel(float volume)
    {
        _AudioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
    public void SetSFXLevel(float volume)
    {
        _AudioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
    /// 
    /// Setting program to fullscreen or turning fullscreen off
    /// 
    public void SetFullScreenMode (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

}
