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

    private LevelDataSO _levelDataSo;
    private Note[] _notesArray;

    private int _spawnIndex;

    private bool _isFirstUpdate = true;
    private void Start()
    {
        _levelDataSo = LevelDataManager.Instance.GetLevelDataSO();

        musicSource.clip = _levelDataSo.audioClip;
        musicSource.Play();
    }

    private void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            GameStateManager.onStateChanged?.Invoke(this, GameStateManager.State.Countdown);
        }
    }
}