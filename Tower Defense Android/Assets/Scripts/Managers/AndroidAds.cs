using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AndroidAds : MonoBehaviour, IUnityAdsListener
{
    private const string gameID = "3590488";
    private const string videoPlacementID = "video";

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, true);
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
}