using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelShader : MonoBehaviour
{
    [SerializeField] float loadingTime = 2f;

    private Material _material;
    private float circleSliderMaxValue=1f;
    private float boundSliderMaxValue=0.5f;
    private const string BOUNDSLIDER = "_BoundSlider";
    private const string CIRCLESLIDER = "_CircleSlider";
    private bool isLoaded = false;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
        _material.SetFloat(BOUNDSLIDER, 0.5f);
        _material.SetFloat(CIRCLESLIDER, 0f);
    }

    public void StartScene()
    {
        StartCoroutine(StartSceneRoutine());       
    }

    public void FinishScene()
    {
        StartCoroutine(FinishSceneRoutine());
    }

    private IEnumerator StartSceneRoutine()
    {
        for (float t = 0; t <= circleSliderMaxValue; t += Time.deltaTime / loadingTime) 
        {
            _material.SetFloat(CIRCLESLIDER, t);
            yield return null;
        }
        _material.SetFloat(CIRCLESLIDER, circleSliderMaxValue);

        for (float i = boundSliderMaxValue; i >= 0f; i -= Time.deltaTime / loadingTime)
        {
            _material.SetFloat(BOUNDSLIDER, i);
            yield return null;
        }

        _material.SetFloat(BOUNDSLIDER, 0);
        isLoaded = !isLoaded;
        gameObject.SetActive(false);
    }

    private IEnumerator FinishSceneRoutine()
    {
        for (float i = 0f; i < boundSliderMaxValue; i += Time.deltaTime / loadingTime) 
        {
            _material.SetFloat(BOUNDSLIDER, i);
            yield return null;
        }

        _material.SetFloat(BOUNDSLIDER, boundSliderMaxValue);

        for (float t = circleSliderMaxValue; t >= 0; t -= Time.deltaTime / loadingTime)
        {
            _material.SetFloat(CIRCLESLIDER, t);
            yield return null;
        }

        _material.SetFloat(CIRCLESLIDER, 0);
        isLoaded = !isLoaded;     
    }

    public float GetLoadingTime()
    {
        return (circleSliderMaxValue + boundSliderMaxValue) * loadingTime;
    }
}
