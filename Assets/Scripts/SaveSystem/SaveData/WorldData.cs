using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WorldData
{
    public SaveData[] saveDatas;

    public WorldData(Saveable[] sav)
    {
        saveDatas = new SaveData[sav.Length];

        for (int i = 0; i < sav.Length; i++)
        {
            saveDatas[i] = new SaveData(sav[i].ID, sav[i].transform.position, sav[i].transform.rotation);
        }
    }
}
