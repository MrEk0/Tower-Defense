using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelButtonsBehaviour : MonoBehaviour
{
    [SerializeField] float timeToLoad = 2f;

    Animator animator;
    int numberOfLevels;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //numberOfLevels = GameManager.GetLevelNumber();
    }

    public void ClickPlayButton()
    {
        //AudioManager.PlayUIButtonAudio();

        //int sceneIndex = GameManager.NumberOfOpenedLevels + 1;
        //StartCoroutine(LoadLevel(sceneIndex));
    }

    public void GoToTheNextLevel()
    {
        //AudioManager.PlayUIButtonAudio();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        //if (numberOfLevels >= nextSceneIndex)
        //{
        //    StartCoroutine(LoadLevel(nextSceneIndex));
        //}
        //else
        //{
        //    StartCoroutine(LoadLevel(0));
        //}
    }

    IEnumerator LoadLevel(int nextSceneIndex)
    {
        //GameManager.PauseGame();

        animator.SetTrigger("LoadLevel");
        yield return new WaitForSeconds(timeToLoad);
        SceneManager.LoadScene(nextSceneIndex);

        if (nextSceneIndex != 0)
        {
            //AudioManager.PlayReadyGoAudio(); ;
        }
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

    //public void LoadSpecificLevel(int levelIndex)
    //{
    //    AudioManager.PlayUIButtonAudio();
    //    StartCoroutine(LoadLevel(levelIndex));
    //}
}

