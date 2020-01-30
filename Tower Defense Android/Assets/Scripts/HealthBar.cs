using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    Camera mainCam;
    RectTransform transformRect;

    public Enemy enemy { private get; set; }

    private void Awake()
    {
        slider=GetComponent<Slider>();
        mainCam = Camera.main;
        transformRect = transform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        slider.maxValue = enemy.GetHealth();
        FollowEnemy();
    } 


    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            FollowEnemy();
            slider.value = enemy.GetHealth();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FollowEnemy()
    {
        Vector2 pointToMove = mainCam.WorldToScreenPoint(enemy.transform.position + new Vector3(0f, 0.5f, 0f));
        //transform.GetComponent<RectTransform>().position = pointToMove;
        transformRect.position = pointToMove;
    }
}
