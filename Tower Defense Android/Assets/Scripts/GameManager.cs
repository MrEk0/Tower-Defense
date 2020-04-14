using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] List<Transform> paths;
    [SerializeField] int numberOfLevels = 20;

    private int currentLevel = 0;
    private List<Transform> wayPoints;
    private Transform pathLastPoint;
    private Transform pathStartPoint;

    //public static event Action<int> onProgressLoaded;
    //public static event Action<float, float> onAudioLoaded;
    public static bool isGamePaused { get; set; } = false;
    public static bool isGameOver { get; set; } = false;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        FindLastAndStartPathPoint();
    }

    //private void Start()
    //{
    //    LoadProgress();
    //}
    //public static void GamePause()
    //{
    //    isGamePaused = true;
    //}

    private static void FindLastAndStartPathPoint()
    {
        instance.wayPoints = new List<Transform>();
        Transform currentPath = instance.paths[instance.currentLevel];

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
        DataSaver.SaveData(AudioManager.SoundVolume, AudioManager.MusicVolume, instance.currentLevel);
    }

    public static void LoadProgress()
    {
        PlayerData progress = DataSaver.LoadData();

        if(progress!=null)
        {
            instance.currentLevel = progress.levelProgress;
            //float soundVol = progress.soundVolume;
            //float musicVol = progress.musicVolume;

            //AudioManager.SetSoundVolume(soundVol);
            //AudioManager.SetMusicVolume(musicVol);

            //onAudioLoaded(musicVol, soundVol);
            //Debug.Log("Load " + soundVol);
        }
 
        //onProgressLoaded(instance.currentLevel);
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
        return instance.paths[instance.currentLevel];
    }

    public static int GetNumberOfLevels()
    {
        return instance.numberOfLevels;
    }

    public static int GetCurrentLevel()
    {
        return instance.currentLevel;
    }

    public static void LevelAccomplished()
    {
        instance.currentLevel++;
        FindLastAndStartPathPoint();
        Save();
    }

    public static void ReloadLevel()
    {
        isGamePaused = false;
        isGameOver = false;
    }
}
