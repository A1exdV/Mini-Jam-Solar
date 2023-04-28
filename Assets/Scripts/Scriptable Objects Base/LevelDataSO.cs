using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData",menuName = "Scriptable Object/new level data")]
public class LevelDataSO: ScriptableObject
{
    public AudioClip audioClip;
    public string midiFileLocation;
    
    public NoteName noteNameUp;
    public NoteName noteNameLeft;
    public NoteName noteNameRight;
    public NoteName noteNameDown;
}