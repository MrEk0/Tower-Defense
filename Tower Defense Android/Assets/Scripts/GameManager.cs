using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] List<Transform> paths;
    [SerializeField] int numberOfLevels = 5;

    private int currentLevel = 1;
    private List<Transform> wayPoints;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static Transform GetLastPathPoint()
    {
        instance.wayPoints = new List<Transform>();
        Transform currentPath = instance.paths[instance.currentLevel];

        int numberOfWayPoints = currentPath.childCount;
        foreach (Transform child in currentPath)
        {
            instance.wayPoints.Add(child);
        }

        return instance.wayPoints[numberOfWayPoints-1];
    }

    public static int GetNumberOfLevels()
    {
        return instance.numberOfLevels;
    }

    public static void LevelAccomplished()
    {
        instance.currentLevel++;
    }
}
