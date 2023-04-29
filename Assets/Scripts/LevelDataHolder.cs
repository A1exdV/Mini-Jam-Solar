using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelDataHolder : MonoBehaviour
{
    public static LevelDataHolder Instance { get; private set; }

    private const string MAIN_GAME_SCENE = "Game";

    public EventHandler OnLevelDataLoading;

    private LevelDataSO _levelDataSo;
    private MidiFile _midiFile;
    private Note[] _notesArray;
    private List<double> _timeStamps;
    private UnityWebRequest _www;

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

        _www = new UnityWebRequest();
    }

    public void LoadNewLevelData(LevelDataSO levelDataSO)
    {
        SetDataFromSO(levelDataSO);
        GetMidiFromPath();
    }


    private void SetDataFromSO(LevelDataSO levelDataSO)
    {
        _levelDataSo = levelDataSO;
        _midiFile = null;
    }

    private void GetMidiFromPath()
    {
        if (_levelDataSo == null)
            throw new ArgumentNullException(_levelDataSo.midiFileLocation);
        if (Application.streamingAssetsPath.StartsWith("http://") ||
            Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        var path = Path.Combine(Application.streamingAssetsPath, _levelDataSo.midiFileLocation);
        var success = false;
        while (!success)
        {
            using (_www = UnityWebRequest.Get(path))
            {
                yield return _www.SendWebRequest();

                if (_www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(_www.error);
                }
                else
                {
                    byte[] results = _www.downloadHandler.data;
                    using (var stream = new MemoryStream(results))
                    {
                        _midiFile = MidiFile.Read(stream);
                        success = true;
                        
                    }
                }
            }
        }
        GetDataFromMidi();
    }

    private void ReadFromFile()
    {
        _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + _levelDataSo.midiFileLocation);
        GetDataFromMidi();
    }

    private void SetTimeStamps()
    {
        _timeStamps = new List<double>();
        foreach (var note in _notesArray)
        {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _midiFile.GetTempoMap());
            _timeStamps.Add((double)metricTimeSpan.TotalMicroseconds / 1000000f);
        }
        SceneManager.LoadScene(MAIN_GAME_SCENE);
    }

    private void GetDataFromMidi()
    {
        var notes = _midiFile.GetNotes();
        _notesArray = new Note[notes.Count];
        notes.CopyTo(_notesArray, 0);

        SetTimeStamps();
    }

    public LevelDataSO GetLevelDataSO()
    {
        return _levelDataSo;
    }

    public Note[] GetNotesArray()
    {
        return _notesArray;
    }

    public double GetTimeStamps(int index)
    {
        return _timeStamps[index];
    }
}