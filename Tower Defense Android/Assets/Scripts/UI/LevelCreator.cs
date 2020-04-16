using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelCreator : MonoBehaviour
{
    [SerializeField] LevelButtonsBehaviour levelButtonsBehaviour;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] GameObject buttonPanelPrefab;
    [SerializeField] GameObject canvas;

    Rect panelRect;
    RectTransform thisRect;
    List<GameObject> buttons;
    int numberPerPage;
    int levelCount = 0;
    int numberOfLevels;

    private void Awake()
    {
        buttons = new List<GameObject>();
        thisRect = GetComponent<RectTransform>();
    }

    //private void OnEnable()
    //{
    //    GameManager.onProgressLoaded += OpenAvailableLevels;
    //}

    //private void OnDisable()
    //{
    //    GameManager.onProgressLoaded -= OpenAvailableLevels;
    //}

    private void Start()
    {
        numberOfLevels = GameManager.GetNumberOfLevels();
        CreateLevelPanel();
        OpenAvailableLevels();
        //GameManager.levelButtons = buttons;
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

        //RectTransformExtensions.SetLeft(panelCloneRect, thisRect.offsetMax.x);
        //RectTransformExtensions.SetRight(panelCloneRect, thisRect.offsetMax.x);
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

            //RectTransformExtensions.SetLeft(panelCloneRect, thisRect.offsetMax.x);
            //RectTransformExtensions.SetRight(panelCloneRect, thisRect.offsetMax.x);
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
        Debug.Log("Reveal");
        float numberOfAvailableLevels=GameManager.GetCurrentLevel();

        for(int i=0; i<=numberOfAvailableLevels; i++)
        {
            buttons[i].GetComponent<LevelButton>().RevealButton();
        }
    }
}

