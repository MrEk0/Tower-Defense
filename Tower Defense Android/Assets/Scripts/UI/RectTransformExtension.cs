using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static Vector2 CalculateScreenBounds(GameObject window, CanvasScaler canvasScaler)
    {
        RectTransform windowRectTransform = window.GetComponent<RectTransform>();

        float windowHeight = windowRectTransform.rect.height;
        float windowWidth = windowRectTransform.rect.width;

        float windowMinPosX = canvasScaler.referenceResolution.x * 0.5f - windowWidth * 0.5f;
        float windowMinPosY = canvasScaler.referenceResolution.y * 0.5f - windowHeight * 0.5f;

        return new Vector2(windowMinPosX, windowMinPosY);
    }

   public static void CheckScreenPosition(GameObject window, float windowMinPosX, float windowMinPosY)
    {
        RectTransform windowRectTransform = window.GetComponent<RectTransform>();

        float currentPosY = windowRectTransform.localPosition.y;
        float currentPosX = windowRectTransform.localPosition.x;

        if (Mathf.Abs(currentPosY) > windowMinPosY)
        {
            float newPosY = currentPosY > 0 ? windowMinPosY : -windowMinPosY;
            windowRectTransform.localPosition = new Vector3(currentPosX, newPosY);
        }
        else if (Mathf.Abs(currentPosX) > windowMinPosX)
        {
            float newPosX = currentPosX > 0 ? windowMinPosX : -windowMinPosX;
            windowRectTransform.localPosition = new Vector3(newPosX, currentPosY);
        }
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

