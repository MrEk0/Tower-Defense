using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI wavesText;
    [SerializeField] GameObject towerPanel;
    [SerializeField] GameObject sellPanel;
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Transform healthBarParent;
    [SerializeField] float startNumberOfCoins;
    [SerializeField] float startAmountOfHealth;

    float coins;
    float lives;
    float wave = 1f;

    public GameObject towerToSell { private get; set; }

    private void OnEnable()
    {
        GetComponent<WaveManager>().onWaveChanged += ChangeWave;
    }

    private void OnDisable()
    {
        GetComponent<WaveManager>().onWaveChanged -= ChangeWave;
    }

    private void Awake()
    {
        livesText.text = "Health " + startAmountOfHealth;
        coinsText.text = "Coins " + startNumberOfCoins;

        coins = startNumberOfCoins;
        lives = startAmountOfHealth;

        Time.timeScale = 1f;
    }

    public void ChangeNumberOfCoins(float cost)
    {
        coins += cost;
        coinsText.text = "Coins " + coins;
    }

    public void GetDamage(float amount)
    {
        //lives = Mathf.Max(lives-amount, 0);
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

    private void ChangeWave(int waveNumber, int wavesCount)
    {
        wavesText.text = "Wave " + (waveNumber + 1) + "/" + wavesCount;
    }

    public void BuyTower(TowerSettings tower)
    {
        float cost = tower.BuildPrice;

        coins -= cost;
        if (coins>=0)
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
        float sellingCost = towerToSell.GetComponent<Tower>().TowerType.BuildPrice / 2;
        coins += Mathf.Round(sellingCost);
        coinsText.text = "Coins " + coins;

        Destroy(towerToSell);
    }

    public void ShowSellPanel()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        sellPanel.SetActive(true);

        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 roundMousePos = new Vector2(Mathf.RoundToInt(screenPoint.x), Mathf.RoundToInt(screenPoint.y));

        Vector2 pointToMove = Camera.main.WorldToScreenPoint(roundMousePos);
        sellPanel.GetComponent<RectTransform>().position = pointToMove;
    }

    public void CreateHealthBar(Enemy enemy)
    {
        GameObject healthBar = Instantiate(healthBarPrefab, healthBarParent, false);
        healthBar.GetComponent<HealthBar>().enemy = enemy;
    }
}
