///////////////////////////////////////////////////////////////////////////////
// File name:		LoadingScreen
//
// Purpose:		    Displays a Loading Screen and loads theh next scene in the background
//
// Related Files:	LoadLoadingScreen.cs
//
// Author:			Justin DaSilva
//
// Created:			5/7/20
//
// Last Modified:	5/10/20
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [Header ("Loading Screen Visuals")]
    [SerializeField] private Image _playerSprite = null;
    [SerializeField] private GameObject _continueText = null;
    [SerializeField] private Text _progressPercentage = null;
    [SerializeField] private Slider _progressBar = null;

    [Header ("Transition")]
    [SerializeField] private Animator _transition = null;

    private Animator _anim = null;
    private bool _startTransition = false;

    public static int SceneToLoad { get; set; }
    static int _loadingSceneBuildIndex = 1;

    public static void LoadScene(int levelNum)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        SceneToLoad = levelNum;
        SceneManager.LoadScene(_loadingSceneBuildIndex);
    }

    private void Start()
    {
        Time.timeScale = 1;
        if (SceneToLoad < 0)
            return;

        if (_startTransition)
            _startTransition = false;

        // start async operation
        _anim = _playerSprite.GetComponent<Animator>();
        StartCoroutine(LoadAsyncOperation());
    }

    private IEnumerator LoadAsyncOperation()
    {
        _transition.SetBool("Start", false);
        yield return new WaitForSecondsRealtime(1.0f);
        // create async operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Single);

        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            // Update Progress Bar
            int progress = (int)Mathf.Clamp01(operation.progress / 0.9f);
            _progressPercentage.text = (progress * 100).ToString() + "%";
            _progressPercentage.resizeTextForBestFit = true;
            _progressBar.value = progress;
            
            // Check if ready to Activate loaded scene
            if (operation.progress >= 0.9f)
            {
                _anim.SetTrigger("Complete");
                if (SceneToLoad == 0)
                {
                    _startTransition = true;
                    _transition.SetBool("Start", true);
                    yield return new WaitForSecondsRealtime(1.0f);
                    break;
                }

                _continueText.SetActive(true);
                if (Input.anyKeyDown)
                {
                    _startTransition = true;
                    _transition.SetBool("Start", true);
                    yield return new WaitForSecondsRealtime(1.0f);
                    break;
                }
            }
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
