using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCell : MonoBehaviour
{
    [SerializeField] GameObject towerWindow;
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (IsPointerOverUIObject())
            return;

        towerWindow.SetActive(true);

        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 roundMousePos = new Vector2(Mathf.RoundToInt(screenPoint.x), Mathf.RoundToInt(screenPoint.y));

        Vector2 pointToMove=Camera.main.WorldToScreenPoint(roundMousePos);
        towerWindow.GetComponent<RectTransform>().position = pointToMove;
        towerWindow.GetComponent<TowerSpawner>().spawnPos = roundMousePos;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
