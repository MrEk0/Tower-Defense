using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    Camera mainCam;
    RectTransform transformRect;

    public Enemy Enemy { private get; set; }

    //private void OnEnable()
    //{
    //    Enemy.onDamageTaken += ChangeSliderValue;
    //}

    //private void OnDisable()
    //{
    //    Enemy.onDamageTaken -= ChangeSliderValue;
    //}
    //private void OnBecameVisible()
    //{
    //    Enemy.onDamageTaken += ChangeSliderValue;
    //}

    //private void OnBecameInvisible()
    //{
    //    Enemy.onDamageTaken -= ChangeSliderValue;
    //}

    private void Awake()
    {
        slider=GetComponent<Slider>();
        mainCam = Camera.main;
        transformRect = transform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        //slider.maxValue = Enemy.GetHealth();
        ////ChangeSliderValue(slider.maxValue);
        //FollowEnemy();
    } 


    // Update is called once per frame
    void Update()
    {
        //if (Enemy)
        //{
        //    FollowEnemy();
        //    slider.value = Enemy.GetHealth();//improve
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public void FollowEnemy(Transform enemyTransform)
    {
        Vector2 pointToMove = mainCam.WorldToScreenPoint(enemyTransform.position + new Vector3(0f, 0.5f, 0f));
        transformRect.position = pointToMove;

        Activate();
    }

    public void ChangeSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        ChangeSliderValue(value);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);
    }
}
