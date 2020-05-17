using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelCreator : MonoBehaviour
{
    [SerializeField] LevelButtonsBehaviour levelButtonsBehaviour;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] GameObject buttonPanelPrefab;
    [SerializeField] GameObject canvas;

    private Rect panelRect;
    private RectTransform thisRect;
    private List<GameObject> buttons;
    private int numberPerPage;
    private int levelCount = 0;
    private int numberOfLevels;

    private void Awake()
    {
        buttons = new List<GameObject>();
        thisRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        numberOfLevels = GameManager.GetNumberOfLevels();
        CreateLevelPanel();
        OpenAvailableLevels();
    }

    private void CreateLevelPanel()
    {
        GameObject panelClone;
        RectTransform panelCloneRect;
        CreateInitializingPanel(out panelClone, out panelCloneRect);

        panelRect = panelCloneRect.rect;
        GridLayoutGroup gridLayout = buttonPanelPrefab.GetComponent<GridLayoutGroup>();
        Vector2 levelButtonSize = gridLayout.cellSize;
        Vector2 spacing = gridLayout.spacing;

        int maxInARow = Mathf.FloorToInt((panelRect.width + spacing.x) / (levelButtonSize.x + spacing.x));
        int maxInAColumn = Mathf.FloorToInt((panelRect.height + spacing.y) / (levelButtonSize.y + spacing.y));
        numberPerPage = maxInARow * maxInAColumn;
        int numberOfPages = Mathf.CeilToInt((float)numberOfLevels / numberPerPage);
        GetComponent<LevelSelector>().NumberOfPages = numberOfPages;
        LoadPanels(numberOfPages);

        Destroy(panelClone);
    }

    private void CreateInitializingPanel(out GameObject panelClone, out RectTransform panelCloneRect)
    {
        panelClone = Instantiate(buttonPanelPrefab);
        panelClone.transform.SetParent(canvas.transform, false);
        panelClone.transform.SetParent(transform);

        panelCloneRect = panelClone.GetComponent<RectTransform>();
        RectTransform thisRect = GetComponent<RectTransform>();

        panelCloneRect.SetLeft(thisRect.offsetMax.x);
        panelCloneRect.SetRight( thisRect.offsetMax.x);
    }

    private void LoadPanels(int numberOfPages)
    {
        for (int i = 0; i <= numberOfPages - 1; i++)
        {
            GameObject panel = Instantiate(buttonPanelPrefab) as GameObject;
            panel.transform.SetParent(canvas.transform, false);
            panel.transform.SetParent(transform);

            RectTransform panelCloneRect = panel.GetComponent<RectTransform>();

            panelCloneRect.SetLeft(thisRect.offsetMax.x);
            panelCloneRect.SetRight(thisRect.offsetMax.x);

            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelRect.width * (i), 0);

            int numberOfButtons = i == numberOfPages - 1 ? numberOfLevels - levelCount : numberPerPage;
            LoadButtons(numberOfButtons, panel);
        }
    }

    private void LoadButtons(int numberOfButtons, GameObject parent)
    {
        for (int i = 0; i <= numberOfButtons - 1; i++)
        {
            levelCount++;
            GameObject button = Instantiate(levelButtonPrefab);

            buttons.Add(button);

            button.transform.SetParent(canvas.transform, false);
            button.transform.SetParent(parent.transform);
            button.GetComponent<LevelButton>().InstantiateButton(levelCount, levelButtonsBehaviour);
        }
    }

    private void OpenAvailableLevels()
    {
        float numberOfAvailableLevels=GameManager.GetCurrentLevel();

        for (int i = 0; i <= numberOfAvailableLevels; i++) 
        {
            buttons[i].GetComponent<LevelButton>().RevealButton();
        }
    }
}

