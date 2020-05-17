using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Tower Update", menuName ="Tower Update")]
public class TowerUpdate :ScriptableObject
{
    [SerializeField] UpdateSettings[] updateSettings=null;

    public float GetRange(TowerClass towerClass, int towerLevel)
    {
        for (int i=0; i<updateSettings.Length; i++)
        {
            if (updateSettings[i].towers == towerClass)
            {
                if (towerLevel > updateSettings[i].range.Length)
                    return updateSettings[i].range[updateSettings[i].range.Length-1];

                return updateSettings[i].range[towerLevel - 1];
            }
        }

        return 0;
    }

    public float GetDamage(TowerClass towerClass, int towerLevel)
    {
        foreach (UpdateSettings setting in updateSettings)
        {
            if (setting.towers == towerClass)
            {
                if (towerLevel > setting.damage.Length)
                    return setting.damage[setting.damage.Length-1];

                return setting.damage[towerLevel - 1];
            }
        }

        return 0;
    }

    public float GetShotInterval(TowerClass towerClass, int towerLevel)
    {
        foreach (UpdateSettings setting in updateSettings)
        {
            if (setting.towers == towerClass)
            {
                if (towerLevel > setting.shotInterval.Length)
                    return setting.shotInterval[setting.shotInterval.Length-1];

                return setting.shotInterval[towerLevel - 1];
            }
        }

        return 0;
    }

    public float GetUpdatePrice(TowerClass towerClass, int towerLevel)
    {
        foreach (UpdateSettings setting in updateSettings)
        {
            if (setting.towers == towerClass)
            {
                if (towerLevel > setting.updatePrice.Length)
                    return 0;

                return setting.updatePrice[towerLevel - 1];
            }
        }

        return 0;
    }

    [Serializable]
    public class UpdateSettings
    {
        public TowerClass towers;
        public float[] updatePrice;
        public float[] range;
        public float[] damage;
        public float[] shotInterval;
    }
}
