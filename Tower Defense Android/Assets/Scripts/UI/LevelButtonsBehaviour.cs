using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelButtonsBehaviour : MonoBehaviour
{
    [SerializeField] float finishSceneTime = 1f;

    //Animator animator;
    LoadPanelShader loadPanel;
    private float _timeToLoad;
    int numberOfLevels;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        loadPanel = GetComponent<LoadPanelShader>();
        _timeToLoad = loadPanel.GetLoadingTime()+finishSceneTime;
    }

    private void Start()
    {
        //numberOfLevels = GameManager.GetLevelNumber();
        loadPanel.StartScene();
    }

    public void ClickPlayButton()
    {
        //AudioManager.PlayUIButtonAudio();

        //int sceneIndex = GameManager.NumberOfOpenedLevels + 1;
        //StartCoroutine(LoadLevel(sceneIndex));
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(nextSceneIndex));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        //GameManager.PauseGame();
      
        loadPanel.FinishScene();
        yield return new WaitForSeconds(_timeToLoad);
        SceneManager.LoadScene(sceneIndex);
    }

    //public void LoadMainMenu()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    StartCoroutine(LoadLevel(0));
    //}

    //public void PlayAgain()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    GameManager.ShowAds();
    //    int levelIndex = SceneManager.GetActiveScene().buildIndex;
    //    StartCoroutine(LoadLevel(levelIndex));
    //}

    //public void PlayPause()
    //{
    //    GameManager.PauseGame();
    //}

    //public void OpenSettingsPanel()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    AudioManager.LoadVolume();
    //}

    //public void CloseSettingButton()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    GameManager.SaveProgress();
    //}

    //public void ResumeGame()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    GameManager.ResumeGame();
    //}

    //public void Quit()
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    Application.Quit();
    //}

    public void LoadSpecificLevel(int levelIndex)
    {
        //AudioManager.PlayUIButtonAudio();
        StartCoroutine(LoadLevel(levelIndex));
    }
}

