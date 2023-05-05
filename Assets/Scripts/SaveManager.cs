using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<bool> openLevelList;

    public List<int> scoreLevelList;

    public float musicVolume;
    public float sfxVolume;

    public SaveData()
    {
        openLevelList = new List<bool>();
        scoreLevelList = new List<int>();
    }
}

public enum SaveDataEnum
{
    OpenLevels,
    ScoreLevels,
    MusicVolume,
    SfxVolume,
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private int levelsCount;


    private SaveData _saveData;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        LoadDataFromPlayerPrefs();
    }

    private void LoadDataFromPlayerPrefs()
    {
        _saveData = new SaveData();
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            print("No Save found!");
            for (var i = 0; i < levelsCount; i++)
            {
                if (i == 0)
                {
                    PlayerPrefs.SetInt("openLevel_" + (i + 1), 1);
                }
                else
                    PlayerPrefs.SetInt("openLevel_" + (i + 1), 0);
                
                PlayerPrefs.SetInt("scoreLevel_" + (i + 1), 0);
            }

            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        for (var i = 0; i < levelsCount; i++)
        {
            _saveData.openLevelList.Add(Convert.ToBoolean(PlayerPrefs.GetInt("openLevel_" + (i + 1))));
            _saveData.scoreLevelList.Add(PlayerPrefs.GetInt("openLevel_" + (i + 1)));
        }

        _saveData.musicVolume = PlayerPrefs.GetFloat("musicVolume");
        _saveData.sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        print("Save loaded!");
    }

    public void SaveDataToPlayerPrefs()
    {
        for (var i = 0; i < levelsCount; i++)
        {
            PlayerPrefs.SetInt("openLevel_" + (i + 1), Convert.ToInt32(_saveData.openLevelList[i]));
            PlayerPrefs.SetInt("scoreLevel_" + (i + 1), _saveData.scoreLevelList[i]);
        }

        PlayerPrefs.SetFloat("musicVolume", _saveData.musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", _saveData.sfxVolume);

        Debug.Log("Data Saved!");
    }

    public void DeleteData()
    {
        for (var i = 0; i < levelsCount; i++)
        {
            if (i == 0)
            {
                PlayerPrefs.SetInt("openLevel_" + (i + 1), 1);
            }
            else
                PlayerPrefs.SetInt("openLevel_" + (i + 1), 0);
            
            PlayerPrefs.SetInt("scoreLevel_" + (i + 1), 0);
        }

        PlayerPrefs.SetFloat("musicVolume", 1);
        PlayerPrefs.SetFloat("sfxVolume", 1);
        
        print("Saves deleted");

        LoadDataFromPlayerPrefs();
    }

    public T GetSaveData<T>(SaveDataEnum type)
    {
        switch (type)
        {
            case SaveDataEnum.OpenLevels:
                return (T)Convert.ChangeType(_saveData.openLevelList, typeof(T));
            case SaveDataEnum.ScoreLevels:
                return (T)Convert.ChangeType(_saveData.scoreLevelList, typeof(T));
            case SaveDataEnum.MusicVolume:
                return (T)Convert.ChangeType(_saveData.musicVolume, typeof(T));
            case SaveDataEnum.SfxVolume:
                return (T)Convert.ChangeType(_saveData.sfxVolume, typeof(T));
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void SetOpenLevel(bool data, int index)
    {
        _saveData.openLevelList[index] = data;
        SaveDataToPlayerPrefs();
    }

    public void SetScoreLevels(int data, int index)
    {
        _saveData.scoreLevelList[index] = data;
        SaveDataToPlayerPrefs();
    }

    public bool IsLevelOpen(int index)
    {
        return _saveData.openLevelList[index];
    }
    public void SetMusicVolume(float data)
    {
        _saveData.musicVolume = data;
        SaveDataToPlayerPrefs();
    }

    public void SetSfxVolume(float data)
    {
        _saveData.sfxVolume = data;
        SaveDataToPlayerPrefs();
    }
}