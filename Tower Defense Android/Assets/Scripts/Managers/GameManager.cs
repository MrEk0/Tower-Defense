using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] List<Transform> paths;
    [SerializeField] int numberOfLevels = 20;

    private int currentLevel = 0;
    private int accomplishedLevels = 0;
    private List<Transform> wayPoints;
    private Transform pathLastPoint;
    private Transform pathStartPoint;
    private AndroidAds ads;

    public static bool isGamePaused { get; set; } = false;
    public static bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        ads = GetComponent<AndroidAds>();
    }

    private static void FindLastAndStartPathPoint()
    {
        instance.wayPoints = new List<Transform>();
        Transform currentPath = instance.paths[instance.currentLevel-1];

        int numberOfWayPoints = currentPath.childCount;
        foreach (Transform child in currentPath)
        {
            instance.wayPoints.Add(child);
        }

        instance.pathLastPoint=instance.wayPoints[numberOfWayPoints-1];
        instance.pathStartPoint = instance.wayPoints[0];
    }

    public static void Save()
    {
        DataSaver.SaveData(AudioManager.SoundVolume, AudioManager.MusicVolume, instance.accomplishedLevels);
    }

    public static void LoadProgress()
    {
        PlayerData progress = DataSaver.LoadData();

        if(progress!=null)
        {
            instance.accomplishedLevels = progress.levelProgress;
        }
    }

    public static Transform GetLastPathPoint()
    {
        return instance.pathLastPoint;
    }

    public static Transform GetStartPoint()
    {
        return instance.pathStartPoint;
    }

    public static Transform GetCurrentPath()
    {
        return instance.paths[instance.currentLevel-1];
    }

    public static int GetNumberOfLevels()
    {
        return instance.numberOfLevels;
    }

    public static int GetCurrentLevel()
    {
        return instance.currentLevel;
    }

    public static int GetAccomplishedLevels() => instance.accomplishedLevels;

    public static void GameOver()
    {
        isGameOver = true;
        AudioManager.StopAllActiveSounds();
        AudioManager.PlayGameOverAudio();
    }

    public static void LevelAccomplished(int currentLevel)
    {
        ShowAds();

        instance.currentLevel=currentLevel;
        instance.accomplishedLevels = currentLevel > instance.accomplishedLevels ? instance.accomplishedLevels+1 : instance.accomplishedLevels;
        FindLastAndStartPathPoint();
        Save();
    }

    public static void ReloadLevel()
    {
        isGamePaused = false;
        isGameOver = false;
    }

    private static void ShowAds()
    {
        instance.ads.ShowVideoAds();
    }
}
