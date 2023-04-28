using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using Note = Melanchall.DryWetMidi.Interaction.Note;

public class LevelGameManager : MonoBehaviour
{

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<NoteName> noteNameList;

    private LevelDataSO _levelDataSo;
    private Note[] _notesArray;

    private int _spawnIndex;


    private void Start()
    {
        _spawnIndex = 0;
        _levelDataSo = LevelDataManager.Instance.GetLevelDataSO();
        musicSource.clip = _levelDataSo.audioClip;
        musicSource.Play();
        _notesArray = LevelDataManager.Instance.GetNotesArray();
    }

    private float GetMusicSourceTime()
    {
        return (float)musicSource.timeSamples/ musicSource.clip.frequency; 
    }

    private void Update()
    {
        while (GetMusicSourceTime() >= LevelDataManager.Instance.GetTimeStamps(_spawnIndex))
        {
            print(_notesArray[_spawnIndex].NoteNumber);
            _spawnIndex++;
        }
    }
}
