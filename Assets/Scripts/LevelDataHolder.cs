using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelDataHolder : MonoBehaviour
{
    public static LevelDataHolder Instance { get; private set; }

    private const string MAIN_GAME_SCENE = "MainGame";

    public EventHandler OnLevelDataLoading;

    private LevelDataSO _levelDataSo;
    private MidiFile _midiFile;
    private Note[] _notesArray;
    private List<double> _timeStamps;

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
    }

    public void LoadNewLevelData(LevelDataSO levelDataSO)
    {
        SetDataFromSO(levelDataSO);
        GetMidiFromPath();
        GetDataFromMidi();
        SetTimeStamps();
    }
    
    
    private void SetDataFromSO(LevelDataSO levelDataSO)
    {
        SceneManager.LoadScene(MAIN_GAME_SCENE);

        _levelDataSo = levelDataSO;
        _midiFile = null;
    }

    private void GetMidiFromPath()
    {
        if (_levelDataSo == null)
            throw new ArgumentNullException(_levelDataSo.midiFileLocation);
        
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
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
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + _levelDataSo.midiFileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    _midiFile = MidiFile.Read(stream);
                }
            }
        }
    }
    private void ReadFromFile()
    {
        _midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + _levelDataSo.midiFileLocation);
    }
    
    private void SetTimeStamps()
    {
        _timeStamps = new List<double>();
        foreach (var note in _notesArray)
        {
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, _midiFile.GetTempoMap());
            _timeStamps.Add((double)metricTimeSpan.TotalMicroseconds / 1000000f);
        }
    }
    
    private void GetDataFromMidi()
    {
        var notes = _midiFile.GetNotes();
        _notesArray = new Note[notes.Count];
        notes.CopyTo(_notesArray, 0);
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
