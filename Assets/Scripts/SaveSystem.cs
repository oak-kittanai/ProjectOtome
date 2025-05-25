using System;
using UnityEngine;

public class SaveSystem
{
    private SaveData[] saveDatas = Array.Empty<SaveData>();

    public SaveData[] GetSaveDatas() => saveDatas;

    public SaveData GetSaveData(int index)
    {
        if (index < 0 || index >= saveDatas.Length)
        {
            Debug.LogError($"Invalid save data index: {index}");
            return null;
        }
        return saveDatas[index];
    }
}

public class SaveData
{
    public DateTime SaveTime { get; set; }
}
