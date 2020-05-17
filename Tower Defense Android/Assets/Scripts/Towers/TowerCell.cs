using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerCell : MonoBehaviour
{
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] GameObject towerWindow;

    Camera cam;
    RectTransform windowRectTransform;
    Vector2 windowScreenBound;

    private void Awake()
    {
        cam = Camera.main;
        windowRectTransform = towerWindow.GetComponent<RectTransform>();
        windowScreenBound=RectTransformExtensions.CalculateScreenBounds(towerWindow, canvasScaler);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (RectTransformExtensions.IsPointerOverUIObject())
            return;

        towerWindow.SetActive(true);

        Vector2 screenPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 roundMousePos = new Vector2(Mathf.RoundToInt(screenPoint.x), Mathf.RoundToInt(screenPoint.y));

        Vector2 pointToMove= cam.WorldToScreenPoint(roundMousePos);
        windowRectTransform.position = pointToMove;

        RectTransformExtensions.CheckScreenPosition(towerWindow, windowScreenBound.x, windowScreenBound.y);
        towerWindow.GetComponent<TowerSpawner>().spawnPos = roundMousePos;
    }
}
