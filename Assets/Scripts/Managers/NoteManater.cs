using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using Note = Melanchall.DryWetMidi.Interaction.Note;

namespace Managers
{
    public class NoteManater : MonoBehaviour
    {
        [SerializeField] private List<NoteName> noteNameList;
    
        private Note[] _notesArray;

        private int _spawnIndex;

        private void Start()
        {
            _spawnIndex = 0;
            _notesArray = LevelDataManager.Instance.GetNotesArray();
        }

        private void Update()
        {
            if(GameStateManager.Instance.GetCurrentState() != GameStateManager.State.Playing)
                return;
        
            while (TimeManager.Instance.GetMusicSourceTime() >= LevelDataManager.Instance.GetTimeStampByIndex(_spawnIndex))
            {
                print(_notesArray[_spawnIndex].NoteName);
                _spawnIndex++;
            }
        }
    }
}
