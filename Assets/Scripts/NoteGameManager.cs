using System;
using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.Serialization;
using Visual;
using Note = Melanchall.DryWetMidi.Interaction.Note;

public class NoteGameManager : MonoBehaviour
{
    public static NoteGameManager Instance { get; private set; }

    [SerializeField] private GameObject notePrefab;
    [SerializeField] private float delayNiceHit = 0.15f;
    [SerializeField] private float delayGreatHit = 0.15f;
    [SerializeField] private float delayPerfectHit = 0.05f;

    [SerializeField] private int scoreByNiceHit = 3;
    [SerializeField] private int scoreByGreatHit = 6;
    [SerializeField] private int scoreByPerfectHit = 10;
    
    
    [SerializeField] private NoteName noteNameUp;
    [SerializeField] private NoteName noteNameLeft;
    [SerializeField] private NoteName noteNameRight;
    [SerializeField] private NoteName noteNameDown;

    [SerializeField] private RectTransform noteSpawnPointUp;
    [SerializeField] private RectTransform noteSpawnPointLeft;
    [SerializeField] private RectTransform noteSpawnPointRight;
    [SerializeField] private RectTransform noteSpawnPointDown;

    private Note[] _notesArray;
    private AudioSource _musicSource;

    private float _spawnTimeAdvance;

    private int _visibleIndex;

    private int _destroyedIndex;
    private int _hitIndexUp;
    private int _hitIndexLeft;
    private int _hitIndexRight;
    private int _hitIndexDown;

    private List<NoteData> _noteDataList;

    private List<NoteData> _noteDataListUp;
    private List<NoteData> _noteDataListLeft;
    private List<NoteData> _noteDataListRight;
    private List<NoteData> _noteDataListDown;

    private int _score;
    private int _multiplier;

    public EventHandler<GameInfo> onPlayerMiss;
    public EventHandler<GameInfo> onPlayerHit;

    public EventHandler<Statistics> onGameEnd;

    private Statistics _statistics;

    private int _perfectHits;
    private int _greatHits;
    private int _niceHits;

    private bool _isGameEnded = false;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialization(AudioSource audioSource, float spawnTimeAdvance)
    {
        _spawnTimeAdvance = spawnTimeAdvance;
        _musicSource = audioSource;
        _visibleIndex = 0;
        _score = 0;
        _multiplier = 0;
        _noteDataList = new List<NoteData>();
        _statistics = new Statistics();

        _noteDataListUp = new List<NoteData>();
        _noteDataListLeft = new List<NoteData>();
        _noteDataListRight = new List<NoteData>();
        _noteDataListDown = new List<NoteData>();


        InputManager.Instance.OnUpAction += InputManager_OnUpAction;
        InputManager.Instance.OnLeftAction += InputManager_OnLeftAction;
        InputManager.Instance.OnRightAction += InputManager_OnRightAction;
        InputManager.Instance.OnDownAction += InputManager_OnDownAction;

        _notesArray = LevelDataHolder.Instance.GetNotesArray();

        InstantiateAllNotes();
    }

    private void InputManager_OnDownAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexDown, _noteDataListDown);
    }

    private void InputManager_OnRightAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexRight, _noteDataListRight);
    }

    private void InputManager_OnLeftAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexLeft, _noteDataListLeft);
    }

    private void InputManager_OnUpAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexUp, _noteDataListUp);
    }

    private void Update()
    {
        if(_isGameEnded)
            return;
        
        CheckForVisibleNotes();
        CheckForMiss();
        CheckForEnd();
    }

    private void CheckForEnd()
    {
        if (_destroyedIndex == _notesArray.Length)
        {
            _statistics.isWin = true;
            _isGameEnded = true;
            onGameEnd?.Invoke(this,_statistics);
        }
    }

    private void CheckForVisibleNotes()
    {
        while (_visibleIndex < _noteDataList.Count &&
               _noteDataList[_visibleIndex].TimeStamp <= GetMusicSourceTime() + _spawnTimeAdvance)
        {
            _noteDataList[_visibleIndex].onNoteMakeVisible
                ?.Invoke(this, _noteDataList[_visibleIndex].TimeStamp - GetMusicSourceTime());
            _visibleIndex++;
        }
    }

    private void CheckForMiss()
    {
        var isMissed = false;
        if (_hitIndexUp < _noteDataListUp.Count &&
            GetMusicSourceTime() >= _noteDataListUp[_hitIndexUp].TimeStamp + delayNiceHit)
        {
            _noteDataListUp[_hitIndexUp].onNoteDestroy?.Invoke(this, EventArgs.Empty);
            _destroyedIndex++;
            print("miss up");
            _hitIndexUp++;
            _statistics.totalNotesMissed++;
            _multiplier = 0;
            isMissed = true;
        }

        if (_hitIndexLeft < _noteDataListLeft.Count &&
            GetMusicSourceTime() >= _noteDataListLeft[_hitIndexLeft].TimeStamp + delayNiceHit)
        {
            _noteDataListLeft[_hitIndexLeft].onNoteDestroy?.Invoke(this, EventArgs.Empty);
            _destroyedIndex++;
            print("miss left");
            _hitIndexLeft++;
            _statistics.totalNotesMissed++;
            _multiplier = 0;
            isMissed = true;
        }

        if (_hitIndexRight < _noteDataListRight.Count &&
            GetMusicSourceTime() >= _noteDataListRight[_hitIndexRight].TimeStamp + delayNiceHit)
        {
            _noteDataListRight[_hitIndexRight].onNoteDestroy?.Invoke(this, EventArgs.Empty);
            _destroyedIndex++;
            print("miss right");
            _hitIndexRight++;
            _statistics.totalNotesMissed++;
            _multiplier = 0;
            isMissed = true;
        }

        if (_hitIndexDown < _noteDataListDown.Count &&
            GetMusicSourceTime() >= _noteDataListDown[_hitIndexDown].TimeStamp + delayNiceHit)
        {
            _noteDataListDown[_hitIndexDown].onNoteDestroy?.Invoke(this, EventArgs.Empty);
            _destroyedIndex++;
            print("miss down");
            _hitIndexDown++;
            _statistics.totalNotesMissed++;
            _multiplier = 0;
            isMissed = true;
        }

        if (isMissed)
        {
            onPlayerMiss?.Invoke(this, GetGameInfo());
        }
    }

    private GameInfo GetGameInfo()
    {
        return new GameInfo
        {
            score = _score,
            multiplier = _multiplier,
            healthNormalized = 0
        };
    }

    private void InstantiateAllNotes()
    {
        _statistics.totalNotes = _notesArray.Length;
        
        for (var index = 0; index < _notesArray.Length; index++)
        {
            var note = _notesArray[index];
            var noteGameObject = Instantiate(notePrefab, Vector3.zero, Quaternion.identity);
            var noteVisual = noteGameObject.GetComponent<NoteVisual>();


            var timeStamp = LevelDataHolder.Instance.GetTimeStamps(index);

            _noteDataList.Add(new NoteData(note, index, timeStamp, noteVisual));
            if (note.NoteName == noteNameUp)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointUp);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;
                _noteDataListUp.Add(_noteDataList[index]);
            }
            else if (note.NoteName == noteNameLeft)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointLeft);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;
                _noteDataListLeft.Add(_noteDataList[index]);
            }
            else if (note.NoteName == noteNameRight)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointRight);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;
                _noteDataListRight.Add(_noteDataList[index]);
            }
            else if (note.NoteName == noteNameDown)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointDown);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;
                _noteDataListDown.Add(_noteDataList[index]);
            }
            else
            {
                throw new ArgumentException($"Note {note.NoteName} not found in the set values.");
            }

            _noteDataList[index].NoteVisual.transform.localScale = Vector3.one;
            noteVisual.SetNoteData(_noteDataList[index]);
        }
    }

    public double GetMusicSourceTime()
    {
        return (double)_musicSource.timeSamples / _musicSource.clip.frequency;
    }

    private void OnInputPerformed(ref int index, List<NoteData> noteDataList)
    {
        if (index >= noteDataList.Count)
        {
            print($"Out of notes. index - {index}, count - {noteDataList.Count} ");
            return;
        }

        var noteData = noteDataList[index];
        if (index < noteDataList.Count && (GetMusicSourceTime() >= noteData.TimeStamp - delayNiceHit &&
                                           GetMusicSourceTime() <= noteData.TimeStamp + delayNiceHit))
        {
            noteData.onNoteDestroy?.Invoke(this, EventArgs.Empty);
            _multiplier++;
            _destroyedIndex++;
            
            _statistics.totalNotesHit++;

            if (GetMusicSourceTime() >= noteData.TimeStamp - delayPerfectHit &&
                GetMusicSourceTime() <= noteData.TimeStamp + delayPerfectHit)
            {
                _statistics.totalPerfectHits++;
                _score += _multiplier* scoreByPerfectHit;
                print("Perfect!");
            }
            else if(GetMusicSourceTime() >= noteData.TimeStamp - delayGreatHit &&
                    GetMusicSourceTime() <= noteData.TimeStamp + delayGreatHit)
            {
                _statistics.totalGreatHits++;
                _score += _multiplier* scoreByGreatHit;
                print("Great!");
            }
            else
            {
                _statistics.totalNiceHits++;
                _score += _multiplier* scoreByNiceHit;
                print("Nice!");
            }
            
            if(_multiplier>_statistics.maxMultiplier)
                _statistics.maxMultiplier = _multiplier;
            
            index++;
            onPlayerHit?.Invoke(this, GetGameInfo());
        }
        else
        {
            print("No notes found");
            onPlayerMiss?.Invoke(this, GetGameInfo());
            _multiplier = 0;
        }
    }
}