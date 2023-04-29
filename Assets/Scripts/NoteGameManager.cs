using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.Serialization;
using Visual;
using Note = Melanchall.DryWetMidi.Interaction.Note;

public class NoteGameManager : MonoBehaviour
{
    public class NoteData
    {
        public Note Note { get; private set; }
        public int Index { get; private set; }
        public double TimeStamp { get; private set; }
        public NoteVisual NoteVisual { get; private set; }

        public EventHandler<double> onNoteMakeVisible;

        public EventHandler onNoteDestroy;

        public NoteData(Note note, int index,double timeStamp,NoteVisual noteVisual )
        {
            Note = note;
            Index = index;
            TimeStamp = timeStamp;
            NoteVisual = noteVisual;
        }
    }
    public static NoteGameManager Instance { get; private set; }
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private float deltaHit = 0.15f;

    private NoteName _noteNameUp;
    private NoteName _noteNameLeft;
    private NoteName _noteNameRight;
    private NoteName _noteNameDown;

    [SerializeField] private Transform noteSpawnPointUp;
    [SerializeField] private Transform noteSpawnPointLeft;
    [SerializeField] private Transform noteSpawnPointRight;
    [SerializeField] private Transform noteSpawnPointDown;

    private LevelDataSO _levelDataSo;
    private Note[] _notesArray;

    private float _spawnTimeAdvance;
    
    private int _visibleIndex;
    private int _hitIndexUp;
    private int _hitIndexLeft;
    private int _hitIndexRight;
    private int _hitIndexDown;
    
    private List<NoteData> _noteDataList;
    
    private List<NoteData> _noteDataListUp;
    private List<NoteData> _noteDataListLeft;
    private List<NoteData> _noteDataListRight;
    private List<NoteData> _noteDataListDown;

    public static int score;
    public static int hitInARow;
    
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

    private void Start()
    {
        _visibleIndex = 0;
        score = 0;
        hitInARow = 0;
        _noteDataList = new List<NoteData>();
        
        _noteDataListUp = new List<NoteData>();
        _noteDataListLeft = new List<NoteData>();
        _noteDataListRight = new List<NoteData>();
        _noteDataListDown = new List<NoteData>();
        
        _levelDataSo = LevelDataHolder.Instance.GetLevelDataSO();

        _noteNameUp = _levelDataSo.noteNameUp;
        _noteNameLeft = _levelDataSo.noteNameLeft;
        _noteNameRight = _levelDataSo.noteNameRight;
        _noteNameDown = _levelDataSo.noteNameDown;
        _spawnTimeAdvance = _levelDataSo.spawnTimeAdvance;
        
        InputManager.Instance.OnUpAction += InputManager_OnUpAction;
        InputManager.Instance.OnLeftAction += InputManager_OnLeftAction;
        InputManager.Instance.OnRightAction += InputManager_OnRightAction;
        InputManager.Instance.OnDownAction += InputManager_OnDownAction;
        
        
        musicSource.clip = _levelDataSo.audioClip;
        musicSource.Play();
        _notesArray = LevelDataHolder.Instance.GetNotesArray();
        InstantiateAllNotes();
    }

    private void InputManager_OnDownAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexDown,_noteDataListDown);
    }

    private void InputManager_OnRightAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexRight,_noteDataListRight);
    }

    private void InputManager_OnLeftAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexLeft,_noteDataListLeft);
    }

    private void InputManager_OnUpAction(object sender, EventArgs e)
    {
        OnInputPerformed(ref _hitIndexUp,_noteDataListUp);
    }

    private void Update()
    {
        CheckForVisibleNotes();
        CheckForMiss();
    }

    private void CheckForVisibleNotes()
    {
        while (_visibleIndex < _noteDataList.Count && _noteDataList[_visibleIndex].TimeStamp <= GetMusicSourceTime() + _spawnTimeAdvance)
        {
            _noteDataList[_visibleIndex].onNoteMakeVisible?.Invoke(this, _noteDataList[_visibleIndex].TimeStamp- GetMusicSourceTime());
            _visibleIndex++;
        }
    }

    private void CheckForMiss()
    {
        if (_hitIndexUp < _noteDataListUp.Count && GetMusicSourceTime() >= _noteDataListUp[_hitIndexUp].TimeStamp + deltaHit)
        {
            _noteDataListUp[_hitIndexUp].onNoteDestroy?.Invoke(this,EventArgs.Empty);
            print("miss up");
            _hitIndexUp++;
            hitInARow = 0;
        }
        if (_hitIndexLeft < _noteDataListLeft.Count && GetMusicSourceTime() >= _noteDataListLeft[_hitIndexLeft].TimeStamp + deltaHit)
        {
            _noteDataListLeft[_hitIndexLeft].onNoteDestroy?.Invoke(this,EventArgs.Empty);
            print("miss left");
            _hitIndexLeft++;
            hitInARow = 0;
        }
        if (_hitIndexRight < _noteDataListRight.Count && GetMusicSourceTime() >= _noteDataListRight[_hitIndexRight].TimeStamp + deltaHit)
        {
            _noteDataListRight[_hitIndexRight].onNoteDestroy?.Invoke(this,EventArgs.Empty);
            print("miss right");
            _hitIndexRight++;
            hitInARow = 0;
        }
        if (_hitIndexDown < _noteDataListDown.Count && GetMusicSourceTime() >= _noteDataListDown[_hitIndexDown].TimeStamp + deltaHit)
        {
            _noteDataListDown[_hitIndexDown].onNoteDestroy?.Invoke(this,EventArgs.Empty);
            print("miss down");
            _hitIndexDown++;
            hitInARow = 0;
        }
        
    }
    private void InstantiateAllNotes()
    {
        for (var index = 0; index < _notesArray.Length; index++)
        {
            var note = _notesArray[index];
            var noteGameObject = Instantiate(notePrefab, Vector3.zero, Quaternion.identity);
            var noteVisual = noteGameObject.GetComponent<NoteVisual>();
            
            
            var timeStamp = LevelDataHolder.Instance.GetTimeStamps(index);
            
            _noteDataList.Add(new NoteData(note,index,timeStamp,noteVisual));
            if (note.NoteName == _noteNameUp)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointUp);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero; 
                _noteDataListUp.Add(_noteDataList[index]);
            }
            else if (note.NoteName == _noteNameLeft)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointLeft);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero; 
                _noteDataListLeft.Add(_noteDataList[index]);
            }
            else if (note.NoteName == _noteNameRight)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointRight);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;                 
                _noteDataListRight.Add(_noteDataList[index]);
            }
            else if (note.NoteName == _noteNameDown)
            {
                _noteDataList[index].NoteVisual.transform.SetParent(noteSpawnPointDown);
                _noteDataList[index].NoteVisual.transform.localPosition = Vector3.zero;                 
                _noteDataListDown.Add(_noteDataList[index]);
            }
            else
            {
                throw new ArgumentException($"Note {note.NoteName} not found in the set values." );
            }
            noteVisual.SetNoteData(_noteDataList[index]);
        }
    }
    
    public double GetMusicSourceTime()
    {
        return (double)musicSource.timeSamples / musicSource.clip.frequency;
    }

    private void OnInputPerformed(ref int index, List<NoteData> noteDataList)
    {
        if (index >= noteDataList.Count)
        {
            print($"Out of notes. index - {index}, count - {noteDataList.Count} ");
            return;
        }
        var noteData = noteDataList[index];
        if (index < noteDataList.Count && (GetMusicSourceTime() >= noteData.TimeStamp - deltaHit &&  GetMusicSourceTime() <= noteData.TimeStamp + deltaHit))
        {
            //noteDataList.Remove(noteData);
            noteData.onNoteDestroy?.Invoke(this,EventArgs.Empty);
            hitInARow++;
            score+=hitInARow;
            print($"Player hit {index}");
            index++;
        }
        else
        {
            print("No notes found");
            hitInARow = 0;
        }
    }
}
