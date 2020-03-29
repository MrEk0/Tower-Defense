using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

    public class LevelButton : MonoBehaviour
    {
        [SerializeField] Image closedImage;
        [SerializeField] TextMeshProUGUI buttonText;

        //private LevelButtonsBehaviour levelButtons;
        private int levelNumber;

        //public void InstantiateButton(int number, LevelButtonsBehaviour levelButtons)
        //{
        //    buttonText.SetText(number.ToString());
        //    this.levelButtons = levelButtons;

        //    levelNumber = number;
        //}

        public void LoadLevel()
        {
            //if (levelButtons == null)
            //    return;

            ////AudioManager.PlayUIButtonAudio();
            //levelButtons.LoadSpecificLevel(levelNumber);
        }

        public void RevealButton()
        {
            GetComponent<Button>().interactable = true;
            closedImage.enabled = false;
        }
    }

