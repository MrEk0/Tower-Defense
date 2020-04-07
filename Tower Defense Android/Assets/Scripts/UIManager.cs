using System;
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
    [SerializeField] GameObject ignoreRaycastPanel;
    [SerializeField] HealthBar healthBarPrefab;
    [SerializeField] Transform healthBarParent;
    [SerializeField] float startNumberOfCoins;
    [SerializeField] float startAmountOfHealth;

    float coins;
    float lives;
    List<HealthBar> healthBars;
    Camera mainCamera;
    Vector2 sellPanelScreenBounds;
    int numberOfHealthBar = 0;

    public GameObject TowerToWork { private get; set; }
    public event Action onTowerUpdated;
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

        Time.timeScale = 1f;//????

        SetUpHealthBars();
        sellPanelScreenBounds = RectTransformExtensions.CalculateScreenBounds(towerPanel, canvasScaler);

        //int screeWidht = Screen.width;
        //int screenHeight = Screen.height;
        //Debug.Log(screenHeight);
    }

    private void SetUpHealthBars()
    {
        healthBars = new List<HealthBar>();
        int numberOfEnemies = WaveManager.Instance.GetNumberOfAllEnemies;

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
    }

    public void GetDamage(float amount)
    {
        lives -= amount;

        if (lives >= 0)
        {
            livesText.text = "Health " + lives;
        }
        else
        {
            livesText.text = "Health 0";

            gameoverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void ChangeWaveText(int waveNumber, int wavesCount)
    {
        wavesText.text = "Wave " + (waveNumber + 1) + "/" + wavesCount;
    }

    public void BuyTower(TowerSettings tower)
    {
        float cost = tower.BuildPrice;

        coins -= cost;
        if (coins >= 0)
        {
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

        Destroy(TowerToWork);//????
    }

    public void UpdateTowerButton()
    {
        float price = TowerToWork.GetComponent<Tower>().GetUpdatePrice();

        if (coins >= price)
        {
            coins -= price;
            coinsText.text = "Coins " + coins;//method
            onTowerUpdated();
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
        //healthBar.Enemy = enemy;
        enemy.HealthBar = healthBar;
        //healthBar.gameObject.SetActive(true);

        numberOfHealthBar++;
    }
}
