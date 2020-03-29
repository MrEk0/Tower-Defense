using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class LevelCreator : MonoBehaviour
    {
        //[SerializeField] LevelButtonsBehaviour levelButtonsBehaviour;
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

        private void Start()
        {
            //numberOfLevels = GameManager.GetLevelNumber();
            CreateLevelPanel();

            //GameManager.levelButtons = buttons;
        }

        private void CreateLevelPanel()
        {
            GameObject panelClone;
            RectTransform panelCloneRect;
            CreateInitializingPanel(out panelClone, out panelCloneRect);

            panelRect = panelCloneRect.rect;
            Rect levelRect = levelButtonPrefab.GetComponent<RectTransform>().rect;
            Vector2 spacing = buttonPanelPrefab.GetComponent<GridLayoutGroup>().spacing;

            int maxInARow = Mathf.FloorToInt((panelRect.width + spacing.x) / (levelRect.width + spacing.x));
            int maxInAColumn = Mathf.FloorToInt((panelRect.height + spacing.y) / (levelRect.height + spacing.y));
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

            RectTransformExtensions.SetLeft(panelCloneRect, thisRect.offsetMax.x);
            RectTransformExtensions.SetRight(panelCloneRect, thisRect.offsetMax.x);
        }

        private void LoadPanels(int numberOfPages)
        {
            for (int i = 0; i <= numberOfPages - 1; i++)
            {
                GameObject panel = Instantiate(buttonPanelPrefab) as GameObject;
                panel.transform.SetParent(canvas.transform, false);
                panel.transform.SetParent(transform);

                RectTransform panelCloneRect = panel.GetComponent<RectTransform>();

                RectTransformExtensions.SetLeft(panelCloneRect, thisRect.offsetMax.x);
                RectTransformExtensions.SetRight(panelCloneRect, thisRect.offsetMax.x);

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
                //button.GetComponent<LevelButton>().InstantiateButton(levelCount, levelButtonsBehaviour);
            }
        }
    }

