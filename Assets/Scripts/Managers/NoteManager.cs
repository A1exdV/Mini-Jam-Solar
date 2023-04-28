using System;
using System.Collections.Generic;
using DG.Tweening;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace Managers
{
    public class NoteManager : MonoBehaviour
    {
        public EventHandler<EventArgsOnNewNoteDraw> onNewNoteDraw;

        public class EventArgsOnNewNoteDraw
        {
            public int index;
            public float lifetime;
        }

        [SerializeField] private GameObject starPrefab;
        [SerializeField] private float timingOffset = 0.2f;


        [Header("Note names")] [SerializeField]
        private NoteName noteNameUp;

        [SerializeField] private NoteName noteNameLeft;
        [SerializeField] private NoteName noteNameRight;
        [SerializeField] private NoteName noteNameDown;

        [Header("Note Spawn points")] [SerializeField]
        private RectTransform noteSpawnPointUp;

        [SerializeField] private RectTransform noteSpawnPointLeft;
        [SerializeField] private RectTransform noteSpawnPointRight;
        [SerializeField] private RectTransform noteSpawnPointDown;


        private float _noteSpawnAdvance;
        private Note[] _notesArray;
        
        private int _spawnIndexUp;
        private int _spawnIndexLeft;
        private int _spawnIndexRight;
        private int _spawnIndexDown;
        private int _activatedNotes;

        private List<(int, StarVisual)> _noteUpList;
        private List<(int, StarVisual)> _noteLeftList;
        private List<(int, StarVisual)> _noteRightList;
        private List<(int, StarVisual)> _noteDownList;


        private void Awake()
        {
            _noteUpList = new List<(int, StarVisual)>();
            _noteLeftList = new List<(int, StarVisual)>();
            _noteRightList = new List<(int, StarVisual)>();
            _noteDownList = new List<(int, StarVisual)>();
        }

        private void Start()
        {
            _noteSpawnAdvance = TimeManager.Instance.GetNoteSpawnAdvance();
            _notesArray = LevelDataManager.Instance.GetNotesArray();
            InstantiateNoteLists();
        }

        private void Update()
        {
            if (GameStateManager.Instance.GetCurrentState() != GameStateManager.State.Playing)
                return;

            DrawNotesOnTime();
            CheckForMissedNotes();
        }

        private void InstantiateNoteLists()
        {
            int index = 0;
            while (index < _notesArray.Length)
            {
                GameObject newStar;

                if (_notesArray[index].NoteName == noteNameUp)
                {
                    newStar = Instantiate(starPrefab, noteSpawnPointUp.position, Quaternion.identity, noteSpawnPointUp);
                    var newStarVisual = newStar.GetComponent<StarVisual>();
                    newStarVisual.Initialisation(index,this);
                    _noteUpList.Add((index, newStarVisual));
                }
                else if (_notesArray[index].NoteName == noteNameLeft)
                {
                    newStar = Instantiate(starPrefab, noteSpawnPointLeft.position, Quaternion.identity,
                        noteSpawnPointLeft);
                    var newStarVisual = newStar.GetComponent<StarVisual>();
                    newStarVisual.Initialisation(index,this);
                    _noteLeftList.Add((index, newStarVisual));
                }
                else if (_notesArray[index].NoteName == noteNameRight)
                {
                    newStar = Instantiate(starPrefab, noteSpawnPointRight.position, Quaternion.identity,
                        noteSpawnPointRight);
                    var newStarVisual = newStar.GetComponent<StarVisual>();
                    newStarVisual.Initialisation(index,this);
                    _noteRightList.Add((index, newStarVisual));
                }
                else if (_notesArray[index].NoteName == noteNameDown)
                {
                    newStar = Instantiate(starPrefab, noteSpawnPointDown.position, Quaternion.identity,
                        noteSpawnPointDown);
                    var newStarVisual = newStar.GetComponent<StarVisual>();
                    newStarVisual.Initialisation(index,this);
                    _noteDownList.Add((index, newStarVisual));
                }
                else
                {
                    throw new ArgumentException("NoteNames mismatch");
                }

                index++;
            }
        }

        private void DrawNotesOnTime()
        {
            var musicTime = TimeManager.Instance.GetMusicSourceTime();
            if (_notesArray.Length > 0 &&
                LevelDataManager.Instance.GetTimeStampByIndex(_activatedNotes) <= musicTime +_noteSpawnAdvance)
            {
                onNewNoteDraw?.Invoke(this,new EventArgsOnNewNoteDraw()
                {
                    index = _activatedNotes,
                    lifetime = LevelDataManager.Instance.GetTimeStampByIndex(_activatedNotes) + timingOffset - musicTime
                });
                _activatedNotes++;
            }

        }

        private void CheckForMissedNotes()
        {
            var musicTime = TimeManager.Instance.GetMusicSourceTime();

            if (_noteUpList.Count > 0 &&
                LevelDataManager.Instance.GetTimeStampByIndex(_noteUpList[0].Item1) + timingOffset < musicTime)
            {
                Destroy(_noteUpList[0].Item2.gameObject);
                _noteUpList.Remove(_noteUpList[0]);
                print("Miss on Up");
            }

            if (_noteLeftList.Count > 0 &&
                LevelDataManager.Instance.GetTimeStampByIndex(_noteLeftList[0].Item1) + timingOffset < musicTime)
            {
                Destroy(_noteLeftList[0].Item2.gameObject);
                _noteLeftList.Remove(_noteLeftList[0]);
                print("Miss on Left");
            }

            if (_noteRightList.Count > 0 &&
                LevelDataManager.Instance.GetTimeStampByIndex(_noteRightList[0].Item1) + timingOffset < musicTime)
            {
                Destroy(_noteRightList[0].Item2.gameObject);
                _noteRightList.Remove(_noteRightList[0]);
                print("Miss on Right");
            }

            if (_noteDownList.Count > 0 &&
                LevelDataManager.Instance.GetTimeStampByIndex(_noteDownList[0].Item1) + timingOffset < musicTime)
            {
                Destroy(_noteDownList[0].Item2.gameObject);
                _noteDownList.Remove(_noteDownList[0]);
                print("Miss on Down");
            }
        }
    }
}