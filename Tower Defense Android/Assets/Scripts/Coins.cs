using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Coins : MonoBehaviour
{
    private float currentNumberOfCoins = 0f;
    TextMeshProUGUI coinText;


    private void Awake()
    {
        coinText=GetComponent<TextMeshProUGUI>();
    }

    public void ChangeText(float numberOfCoins)
    {
        currentNumberOfCoins += numberOfCoins;
        coinText.text = currentNumberOfCoins.ToString();
    }
}
