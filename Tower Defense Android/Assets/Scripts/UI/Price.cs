using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Price : MonoBehaviour
{
    [SerializeField] TowerSettings towerType;
    TextMeshProUGUI priceText;

    private void Awake()
    {
        priceText = GetComponent<TextMeshProUGUI>();
        priceText.text = towerType.BuildPrice.ToString();
    }
}
