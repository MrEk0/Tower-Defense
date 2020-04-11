﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SliderType
{
    MusicSlider,
    SoundSlider
}

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] SliderType sliderType;
    [SerializeField] Image soundImage;
    //[SerializeField] Sprite soundOnSprite;
    [SerializeField] Sprite soundOffSprite;

    private float minSliderValue;
    private Slider slider;
    private Sprite soundOnSprite;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        minSliderValue = slider.minValue;
        soundOnSprite = soundImage.sprite;
    }

    //private void OnEnable()
    //{
    //    Debug.Log("Enable");
    //    //AudioManager.onAudioLoaded += SetSliderValue;
    //}

    //private void OnDisable()
    //{
    //    AudioManager.onAudioLoaded -= SetSliderValue;
    //}
    //private void OnBecameVisible()
    //{
    //    Debug.Log("Visible");
    //    SetSliderValue();
    //}
    private void Start()
    {
        SetSliderValue();
    }

    public void SetSoundVolume(float volume)
    {
        //slider.value = volume;
        AudioManager.SetSoundVolume(volume);

        ReplaceSoundSprite();
    }

    public void SetMusicVolume(float volume)
    {
        //slider.value = volume;
        AudioManager.SetMusicVolume(volume);

        ReplaceSoundSprite();

    }

    private void ReplaceSoundSprite()
    {
        if (slider.value == minSliderValue)
        {
            soundImage.sprite = soundOffSprite;
        }
        else
        {
            soundImage.sprite = soundOnSprite;
        }
    }

   public void SetSliderValue(/*float musicVolume, float soundVolume*/)
    {
        float musicVolume=AudioManager.MusicVolume;
        float soundVolume = AudioManager.SoundVolume;

        switch(sliderType)
        {
            case SliderType.MusicSlider:
                slider.value = musicVolume;
                break;
            case SliderType.SoundSlider:
                slider.value = soundVolume;
                break;
        }
    }
}
