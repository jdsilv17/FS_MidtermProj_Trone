////////////////////////////////////////////////////////////////////////////////
// File name:		LoadSaveJSON
//
// Purpose:		    Saving and Loading player data
//
// Related Files:	PlayerController2D.cs, Pickup.cs, SettingsMenu.cs
//
// Author:			Amara Gitomer 
//
// Created:			5/11/20
//
// Last Modified:	 5/11/20
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class LoadSaveJSON : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }
    public void Save()
    {
        //Grabbing Settings From System
        IOSettings file = new IOSettings();
        file.FullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;
        file.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
        file.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        file.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        file.ResolutionWidth = PlayerPrefs.GetFloat("ResolutionWidth");
        file.ResolutionHeight = PlayerPrefs.GetFloat("ResolutionHeight");

        //converting to JSON
        string jsonSettingData = JsonUtility.ToJson(file);

        //Saving JSON string
        PlayerPrefs.SetString("GameSettings", jsonSettingData);
        PlayerPrefs.Save();

        //Grabbing Upgrades From System
        IOPlayerUpgrades file2 = new IOPlayerUpgrades();
        file2._hasDash = PlayerPrefs.GetInt("Dash") == 1 ? true : false;
        file2._hasDoubleJump = PlayerPrefs.GetInt("DoubleJump") == 1 ? true : false;
        file2._hasWallJump = PlayerPrefs.GetInt("WallJump") == 1 ? true : false;
        file2._hasWrenchThrow = PlayerPrefs.GetInt("WrenchThrow") == 1 ? true : false;
        file2._hasShield = PlayerPrefs.GetInt("Shield") == 1 ? true : false;

        //Converting to JSON
        string jsonPlayerUpgradeData = JsonUtility.ToJson(file2);

        //Saving JSON string
        PlayerPrefs.SetString("CurrentPlayerUpgrades", jsonPlayerUpgradeData);
        PlayerPrefs.Save();

    }

    public void Load()
    {
        //Load Saved JSON for Settings
        string jsonSettingLoadData = PlayerPrefs.GetString("GameSettings");

        //Convert to Class
        IOSettings loadedSettingsData = JsonUtility.FromJson<IOSettings>(jsonSettingLoadData);


        //Load Saved JSON for Upgrades
        string jsonUpgradesLoadData = PlayerPrefs.GetString("CurrentPlayerUpgrades");
        IOPlayerUpgrades loadedUpgradesData = JsonUtility.FromJson<IOPlayerUpgrades>(jsonUpgradesLoadData);


        //Load Saved JSON for Rooms

        //Debug Log Displayed for Rooms
    }
}

//Current Settings Storage
public class IOSettings
{
    public float MasterVolume = 0;
    public float MusicVolume = 0;
    public float SFXVolume = 0;
    public bool FullScreen = true;
    public float ResolutionWidth = 0;
    public float ResolutionHeight = 0;

}

//Current Upgrades Storage
public class IOPlayerUpgrades
{
    public bool _hasDash = false;
    public bool _hasDoubleJump = false;
    public bool _hasWallJump = false;
    public bool _hasWrenchThrow = false;
    public bool _hasShield = false;

}

//Current Room Storage
public class IORooms
{

}
