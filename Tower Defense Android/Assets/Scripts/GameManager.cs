﻿using System.Collections;
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

    //public static event Action<int> onProgressLoaded;
    //public static event Action<float, float> onAudioLoaded;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        FindLastPathPoint();
    }

    //private void Start()
    //{
    //    LoadProgress();
    //}

    private static void FindLastPathPoint()
    {
        instance.wayPoints = new List<Transform>();
        Transform currentPath = instance.paths[instance.currentLevel];

        int numberOfWayPoints = currentPath.childCount;
        foreach (Transform child in currentPath)
        {
            instance.wayPoints.Add(child);
        }

        instance.pathLastPoint=instance.wayPoints[numberOfWayPoints-1];
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
        FindLastPathPoint();
        Save();
    }
}
