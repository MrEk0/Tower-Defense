using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSaver
{
    const string FILENAME = "save.data";

    public static void SaveData(float soundVolume, float musicVolume, int levelProgress)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GetFilePath();

        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(soundVolume, musicVolume);

        formatter.Serialize(stream, playerData);
        stream.Close();

    }

    //public static void SaveData(/*float soundVolume, float musicVolume,*/ int levelProgress)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = GetFilePath();

    //    FileStream stream = new FileStream(path, FileMode.Create);
    //    PlayerData playerData = new PlayerData(/*soundVolume, musicVolume,*/ levelProgress);

    //    formatter.Serialize(stream, playerData);
    //    stream.Close();

    //}

    public static PlayerData LoadData()
    {
        string path = GetFilePath();

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return playerData;
        }
        else
        {
            return null;
        }
    }

    private static string GetFilePath()
    {
        string filePath = Path.Combine(Application.persistentDataPath, FILENAME);
        return filePath;
    }
}
