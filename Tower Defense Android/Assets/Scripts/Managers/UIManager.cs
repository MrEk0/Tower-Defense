﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[DefaultExecutionOrder(-50)]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI wavesText;
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] GameObject towerPanel;
    [SerializeField] GameObject updatePanel;
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject ignoreRaycastPanel;
    [SerializeField] HealthBar healthBarPrefab;
    [SerializeField] Transform healthBarParent;
    [SerializeField] float startNumberOfCoins;
    [SerializeField] float startAmountOfHealth;

    private float coins;
    private float lives;
    private List<HealthBar> healthBars;
    private Camera mainCamera;
    private Vector2 sellPanelScreenBounds;
    private int numberOfHealthBar = 0;

    public Tower TowerToWork { private get; set; }

    public event Action<float> onUpdatePanelShowed;

    private void OnEnable()
    {
        WaveManager.Instance.onWaveChanged += ChangeWaveText;
    }

    private void OnDisable()
    {
        WaveManager.Instance.onWaveChanged -= ChangeWaveText;
    }

    private void Awake()
    {
        Instance = this;

        mainCamera = Camera.main;

        livesText.text = "Health " + startAmountOfHealth;
        coinsText.text = "Coins " + startNumberOfCoins;

        coins = startNumberOfCoins;
        lives = startAmountOfHealth;

        SetUpHealthBars();
        sellPanelScreenBounds = RectTransformExtensions.CalculateScreenBounds(towerPanel, canvasScaler);
    }

    private void SetUpHealthBars()
    {
        healthBars = new List<HealthBar>();
        int numberOfEnemies = WaveManager.Instance.GetNumberOfAllEnemies();

        for (int i=0; i<numberOfEnemies; i++)
        {
            HealthBar healthBar = Instantiate(healthBarPrefab, healthBarParent, false);
            healthBar.gameObject.SetActive(false);
            healthBars.Add(healthBar);
        }
    }

    public void ChangeNumberOfCoins(float cost)
    {
        coins += cost;
        coinsText.text = "Coins " + coins;
        AudioManager.PlayPickUpAudio();
    }

    public void GetDamage(float amount)
    {
        lives=Math.Max(0, lives - amount);
        livesText.text = "Health " + lives;

        if(lives==0)
        {
            ignoreRaycastPanel.SetActive(true);
            gameoverPanel.SetActive(true);
            GameManager.GameOver();
        }
    }

    private void ChangeWaveText(int waveNumber, int numberOfWaves)
    {
        wavesText.text = "Wave " + (waveNumber + 1) + "/" + numberOfWaves;
    }

    public void BuyTower(TowerSettings tower)
    {
        AudioManager.PlayUIButtonAudio();

        float cost = tower.BuildPrice;
    
        if (coins >= cost)
        {
            coins -= cost;
            coinsText.text = "Coins " + coins;
        }
        else
        {
            towerPanel.GetComponent<TowerSpawner>().canSpawn = false;
        }
    }

    public void SellTower()
    {
        float sellingCost = TowerToWork.GetComponent<Tower>().GetBuildPrice() / 2;
        coins += Mathf.Round(sellingCost);
        coinsText.text = "Coins " + coins;

        AudioManager.PlayUIButtonAudio();
        Destroy(TowerToWork.gameObject);
    }

    public void UpdateTowerButton()
    {
        float price = TowerToWork.GetComponent<Tower>().GetUpdatePrice();

        if (coins >= price)
        {
            coins -= price;
            coinsText.text = "Coins " + coins;

            TowerToWork.UpdateTower();
            AudioManager.PlayUIButtonAudio();
        }
    }

    public void ShowUpdatePanel()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        ignoreRaycastPanel.SetActive(true);
        updatePanel.SetActive(true);
      
        Vector2 screenPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 roundMousePos = new Vector2(Mathf.RoundToInt(screenPoint.x), Mathf.RoundToInt(screenPoint.y));

        Vector2 pointToMove = mainCamera.WorldToScreenPoint(roundMousePos);
        updatePanel.GetComponent<RectTransform>().position = pointToMove;

        RectTransformExtensions.CheckScreenPosition(towerPanel, sellPanelScreenBounds.x, sellPanelScreenBounds.y);

        float updatePrice = TowerToWork.GetComponent<Tower>().GetUpdatePrice();
        onUpdatePanelShowed(updatePrice);
    }

    public void InitializeHealthBar(Enemy enemy)
    {
        HealthBar healthBar = healthBars[numberOfHealthBar];
        enemy.HealthBar = healthBar;

        numberOfHealthBar++;
    }

    public void ShowWinPanel()
    {
        ignoreRaycastPanel.SetActive(true);
        winPanel.SetActive(true);
        AudioManager.PlayCongratulationsAudio();
    }

    public void CloseRaycastPanel()
    {
        ignoreRaycastPanel.SetActive(false);
    }
}
