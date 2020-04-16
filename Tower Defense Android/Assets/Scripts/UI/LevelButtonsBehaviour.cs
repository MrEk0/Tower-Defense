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

        GameManager.LoadProgress();
        AudioManager.LoadVolume();
    }

    private void Start()
    {
        //numberOfLevels = GameManager.GetLevelNumber();
        loadPanel.StartScene();
        GameManager.ReloadLevel();
        //GameManager.LoadProgress();
        //AudioManager.LoadVolume();
    }

    public void ClickPlayButton()
    {
        AudioManager.PlayUIButtonAudio();

        int sceneIndex = GameManager.GetCurrentLevel() + 1;
        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(sceneIndex));
        GameManager.Save();
    }

    public void LoadNextLevel()
    {
        AudioManager.PlayUIButtonAudio();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(nextSceneIndex));
        GameManager.LevelAccomplished();
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        //GameManager.PauseGame();
      
        loadPanel.FinishScene();
        yield return new WaitForSeconds(_timeToLoad);
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadMainMenu()
    {
        AudioManager.PlayUIButtonAudio();
        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(0));
        GameManager.Save();
    }

    public void PlayAgain()
    {
        AudioManager.PlayUIButtonAudio();
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(levelIndex));
    }

    public void PlayPause()
    {
        AudioManager.PlayUIButtonAudio();
        AudioManager.StopAllActiveSounds();
        GameManager.isGamePaused = true;
        //GameManager.PauseGame();
    }

    public void ResumeGame()
    {
        AudioManager.PlayUIButtonAudio();
        //AudioManager.PlayActiveSounds();
        GameManager.isGamePaused = false;
        //GameManager.ResumeGame();
    }

    public void Quit()
    {
        AudioManager.PlayUIButtonAudio();
        Application.Quit();
    }

    public void LoadSpecificLevel(int levelIndex)
    {
        AudioManager.PlayUIButtonAudio();
        loadPanel.gameObject.SetActive(true);
        StartCoroutine(LoadLevel(levelIndex));
        GameManager.Save();
    }
}

