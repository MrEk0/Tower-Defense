using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Ads : MonoBehaviour, IUnityAdsListener
{
    private const string gameID = "3491898";
    private const string videoPlacementID = "video";

    IEnumerator Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID);

        while (!Advertisement.IsReady())
            yield return null;

        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    public void ShowVideoAds()
    {
        Advertisement.Show(videoPlacementID);
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {
        if (placementId == videoPlacementID)
        {
            Time.timeScale = 0;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished && placementId == videoPlacementID)
        {
            Time.timeScale = 1;
        }
        else if (showResult == ShowResult.Skipped && placementId == videoPlacementID)
        {
            Time.timeScale = 1;
        }
    }

    public void CloseBanner()
    {
        Advertisement.Banner.Hide();
    }
}