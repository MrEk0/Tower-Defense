using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelButtonsBehaviour : MonoBehaviour
{
    [SerializeField] float finishSceneTime = 1f;

    private Animator animator;
    private const string FINISHTRIGGER = "Finish";

    private void Awake()
    {
        animator = GetComponent<Animator>();

        GameManager.LoadProgress();
    }

    private void Start()
    {
        GameManager.ReloadLevel();
        animator.ResetTrigger(FINISHTRIGGER);
        AudioManager.LoadVolume();
    }

    public void ClickPlayButton()
    {
        AudioManager.PlayUIButtonAudio();

        int sceneIndex = GameManager.GetCurrentLevel() + 1;
        StartCoroutine(LoadLevel(sceneIndex));
        GameManager.Save();
        GameManager.LevelAccomplished(sceneIndex);
    }

    public void LoadNextLevel()
    {
        AudioManager.PlayUIButtonAudio();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        StartCoroutine(LoadLevel(nextSceneIndex));
        GameManager.LevelAccomplished(nextSceneIndex);
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        animator.SetTrigger(FINISHTRIGGER);
        yield return new WaitForSeconds(finishSceneTime);
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadMainMenu()
    {
        AudioManager.PlayUIButtonAudio();
        StartCoroutine(LoadLevel(0));
        GameManager.Save();
    }

    public void PlayAgain()
    {
        AudioManager.PlayUIButtonAudio();
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadLevel(levelIndex));
    }

    public void PlayPause()
    {
        AudioManager.PlayUIButtonAudio();
        AudioManager.StopAllActiveSounds();
        GameManager.isGamePaused = true;
    }

    public void ResumeGame()
    {
        UIManager.Instance.CloseRaycastPanel();
        AudioManager.PlayUIButtonAudio();
        GameManager.isGamePaused = false;
    }

    public void Quit()
    {
        AudioManager.PlayUIButtonAudio();
        Application.Quit();
    }

    public void LoadSpecificLevel(int levelIndex)
    {
        AudioManager.PlayUIButtonAudio();
        StartCoroutine(LoadLevel(levelIndex));
        GameManager.Save();
        GameManager.LevelAccomplished(levelIndex);
    }
}

