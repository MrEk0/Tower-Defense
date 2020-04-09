using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public float soundVolume;
    public float musicVolume;
    public int levelProgress;

    public PlayerData(float soundVolume, float musicVolume)
    {
        this.soundVolume = soundVolume;
        this.musicVolume = musicVolume;
    }

    public PlayerData(/*float soundVolume, float musicVolume,*/ int levelProgress)
    {
        //this.soundVolume = soundVolume;
        //this.musicVolume = musicVolume;
        this.levelProgress = levelProgress;
    }
}
