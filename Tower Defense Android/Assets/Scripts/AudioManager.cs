using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;

    [Header("Ambient Clips")]
    public AudioClip mainMusicClip;
    public AudioClip clickUIClip;
    public AudioClip swipeUIClip;

    [Header("Enemy Clips")]
    public AudioClip tankClip;
    public AudioClip greyAirplaneClip;
    public AudioClip greenAirplaneClip;
    public AudioClip stormtrooperStepClip;
    public AudioClip soldierStepClip;
    public AudioClip desertSoldierStepClip;

    [Header("Impact Clips")]
    public AudioClip explosionClip;
    public AudioClip metalHitClip;
    public AudioClip bodyHitClip;
    public AudioClip coinPickUpClip;

    [Header("Tower Shots")]
    public AudioClip rocketShotClip;
    public AudioClip bulletShotclip;

    [Header("Voice Clips")]
    public AudioClip gameOverClip;
    public AudioClip congratulationsClip;

    [Header("Mixer Groups")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup soundGroup;
    public AudioMixerGroup towerGroup;
    public AudioMixerGroup enemyGroup;
    public AudioMixerGroup voiceGroup;
    public AudioMixerGroup stringGroup;

    AudioSource musicSource;
    AudioSource enemyBodyImpactSource;
    AudioSource enemyMetalImpactSource;
    AudioSource tankSource;
    AudioSource greyAirplaneSource;
    AudioSource greenAirplaneSource;
    AudioSource stormtrooperSource;
    AudioSource soldierSource;
    AudioSource desertSoldierSource;
    AudioSource enemyExplosionSource;
    AudioSource bulletTowerSource;
    AudioSource rocketTowerSource;
    AudioSource voiceSource;
    //AudioSource rocketSource;
    AudioSource stingSource;

    public static float MusicVolume { get; private set; }
    public static float SoundVolume { get; private set; }

    public static event Action<float, float> onAudioLoaded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        enemyExplosionSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        enemyBodyImpactSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        voiceSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        bulletTowerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        enemyMetalImpactSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        tankSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        greyAirplaneSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        greenAirplaneSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        soldierSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        desertSoldierSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        stormtrooperSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        rocketTowerSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        stingSource = gameObject.AddComponent<AudioSource>() as AudioSource;

        musicSource.outputAudioMixerGroup = musicGroup;
        enemyBodyImpactSource.outputAudioMixerGroup = towerGroup;
        enemyExplosionSource.outputAudioMixerGroup = enemyGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;
        bulletTowerSource.outputAudioMixerGroup = towerGroup;
        enemyMetalImpactSource.outputAudioMixerGroup = enemyGroup;
        tankSource.outputAudioMixerGroup = enemyGroup;
        greyAirplaneSource.outputAudioMixerGroup = enemyGroup;
        greenAirplaneSource.outputAudioMixerGroup = enemyGroup;
        stormtrooperSource.outputAudioMixerGroup = enemyGroup;
        soldierSource.outputAudioMixerGroup = enemyGroup;
        desertSoldierSource.outputAudioMixerGroup = enemyGroup;
        rocketTowerSource.outputAudioMixerGroup = towerGroup;
        stingSource.outputAudioMixerGroup = stringGroup;

        PlayMainAudio();
    }

    //private void Start()
    //{
    //    LoadVolume();
    //}

    private void PlayMainAudio()
    {
        if (instance == null)
            return;

        instance.musicSource.clip = mainMusicClip;
        instance.musicSource.loop = true;
        instance.musicSource.Play();
    }

    public static void StopAllSounds()
    {
        //instance.soundGroup.audioMixer.SetFloat("Sound", 0f);
        //instance.tankPlayerSource.Stop();
        
    }



    public static void PlayTankAudio()
    {
        if (instance == null || instance.tankSource.isPlaying)
            return;

        instance.tankSource.clip = instance.tankClip;
        instance.tankSource.loop = true;
        instance.tankSource.Play();
    }

    public static void PlayGreyAirplaneAudio()
    {
        if (instance == null ||instance.greyAirplaneSource.isPlaying)
            return;

        instance.greyAirplaneSource.clip = instance.greyAirplaneClip;
        instance.greyAirplaneSource.loop = true;
        instance.greyAirplaneSource.Play();
    }

    public static void PlayGreenAirplaneAudio()
    {
        if (instance == null || instance.greenAirplaneSource.isPlaying)
            return;

        instance.greenAirplaneSource.clip = instance.greenAirplaneClip;
        instance.greenAirplaneSource.loop = true;
        instance.greenAirplaneSource.Play();
    }

    public static void PlaySoldierAudio()
    {
        if (instance == null || instance.soldierSource.isPlaying)
            return;

        instance.soldierSource.clip = instance.soldierStepClip;
        instance.soldierSource.loop = true;
        instance.soldierSource.Play();
    }

    public static void PlayStormtrooperAudio()
    {
        if (instance == null || instance.stormtrooperSource.isPlaying)
            return;

        instance.stormtrooperSource.clip = instance.stormtrooperStepClip;
        instance.stormtrooperSource.loop = true;
        instance.stormtrooperSource.Play();
    }

    public static void PlayDesertSoldierAudio()
    {
        if (instance == null || instance.desertSoldierSource.isPlaying)
            return;

        instance.desertSoldierSource.clip = instance.desertSoldierStepClip;
        instance.desertSoldierSource.loop = true;
        instance.desertSoldierSource.Play();
    }

    public static void PlayGameOverAudio()
    {
        if (instance == null)
            return;

        StopAllSounds();

        instance.voiceSource.clip = instance.gameOverClip;
        instance.voiceSource.Play();
    }

    public static void PlayCongratulationsAudio()
    {
        if (instance == null)
            return;

        StopAllSounds();

        instance.voiceSource.clip = instance.congratulationsClip;
        instance.voiceSource.Play();
    }

    public static void PlayUIButtonAudio()
    {
        if (instance == null)
            return;

        instance.stingSource.clip = instance.clickUIClip;
        instance.stingSource.Play();
    }

    public static void PlayUISwipeAudio()
    {
        if (instance == null)
            return;

        instance.stingSource.clip = instance.swipeUIClip;
        instance.stingSource.Play();
    }

    public static void PlayEnemyExplosionAudio()
    {
        if (instance == null)
            return;

        instance.enemyExplosionSource.clip = instance.explosionClip;
        //instance.enemySource.loop = false;
        instance.enemyExplosionSource.Play();
    }

    public static void PlayRocketTowerFireAudio()
    {
        if (instance == null)
            return;

        instance.rocketTowerSource.clip = instance.rocketShotClip;
        instance.rocketTowerSource.Play();
    }

    public static void PlayBulletTowerFireAudio()
    {
        if (instance == null)
            return;

        instance.bulletTowerSource.clip = instance.bulletShotclip;
        instance.bulletTowerSource.Play();
    }

    public static void PlayEnemyMetalHitAudio()
    {
        if (instance == null)
            return;

        instance.enemyMetalImpactSource.clip = instance.metalHitClip;
        instance.enemyMetalImpactSource.Play();
    }

    public static void PlayBodyHitAudio()
    {
        if (instance == null)
            return;

        instance.enemyBodyImpactSource.clip = instance.bodyHitClip;
        instance.enemyBodyImpactSource.Play();
    }

    public static void PlayPickUpAudio()
    {
        if (instance == null)
            return;

        instance.stingSource.clip = instance.coinPickUpClip;
        instance.stingSource.Play();
    }

    public static void SetSoundVolume(float volume)
    {
        SoundVolume = volume;
        instance.soundGroup.audioMixer.SetFloat("Sound", volume);
    }

    public static void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        instance.musicGroup.audioMixer.SetFloat("Music", volume);
    }

    public static void LoadVolume()
    {
        PlayerData data = DataSaver.LoadData();

        if (data != null)
        {

            SetMusicVolume(data.musicVolume);
            SetSoundVolume(data.soundVolume);
            //onAudioLoaded(MusicVolume, SoundVolume);
        }
    }
}

