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
    [SerializeField] private float spawnTimeAdvance = 0.5f;

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

    private int _visibleIndex;
    private int _hitIndex;
    
    private List<NoteData> _noteDataList;
    
    private List<NoteData> _noteDataListUp;
    private List<NoteData> _noteDataListLeft;
    private List<NoteData> _noteDataListRight;
    private List<NoteData> _noteDataListDown;
    
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
        
        
        musicSource.clip = _levelDataSo.audioClip;
        musicSource.Play();
        _notesArray = LevelDataHolder.Instance.GetNotesArray();
        InstantiateAllNotes();
    }
    private void Update()
    {
        while (_visibleIndex < _noteDataList.Count && _noteDataList[_visibleIndex].TimeStamp <= GetMusicSourceTime() + spawnTimeAdvance)
        {
            _noteDataList[_visibleIndex].onNoteMakeVisible?.Invoke(this, spawnTimeAdvance);
            _visibleIndex++;
        }
        
        
        //temp on hit time 
        while (_hitIndex < _noteDataList.Count && GetMusicSourceTime() >= _noteDataList[_hitIndex].TimeStamp)
        {
            _noteDataList[_hitIndex].onNoteDestroy?.Invoke(this,EventArgs.Empty);
            print("Destroyed");
            _hitIndex++;
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
}
