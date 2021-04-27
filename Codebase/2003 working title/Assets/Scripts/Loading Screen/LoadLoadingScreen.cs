///////////////////////////////////////////////////////////////////////////////
// File name:		LoadLoadingScreen
//
// Purpose:		    Loads in the Loading Screen
//
// Related Files:	LoadingScreen.cs
//
// Author:			Justin DaSilva
//
// Created:			5/10/20
//
// Last Modified:	5/10/20
///////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLoadingScreen : MonoBehaviour
{
    [SerializeField] private Animator _transition = null;
    public int SceneToLoad { get; set; }

    public void StartLoading()
    {
        StartCoroutine(PlayTransition());
    }
    
    private IEnumerator PlayTransition()
    {
        _transition.SetBool("Start", true);
        yield return new WaitForSecondsRealtime(1.0f);
        LoadScene();
    }

    private void LoadScene()
    {
        if (SceneToLoad < 0 || SceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            return;
        }
        LoadingSceneManager.LoadScene(SceneToLoad);
    }
}
