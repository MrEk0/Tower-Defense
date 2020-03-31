using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] List<Transform> paths;
    [SerializeField] int numberOfLevels = 20;

    private int currentLevel = 0;
    private List<Transform> wayPoints;
    private Transform pathLastPoint;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        FindLastPathPoint();
    }

    private static void FindLastPathPoint()
    {
        instance.wayPoints = new List<Transform>();
        Transform currentPath = instance.paths[instance.currentLevel];

        int numberOfWayPoints = currentPath.childCount;
        foreach (Transform child in currentPath)
        {
            instance.wayPoints.Add(child);
        }

        instance.pathLastPoint=instance.wayPoints[numberOfWayPoints-1];
    }

    public static Transform GetLastPathPoint()
    {
        return instance.pathLastPoint;
    }

    public static Transform GetCurrentPath()
    {
        return instance.paths[instance.currentLevel];
    }

    public static int GetNumberOfLevels()
    {
        return instance.numberOfLevels;
    }

    public static void LevelAccomplished()
    {
        instance.currentLevel++;
        FindLastPathPoint();
    }
}
