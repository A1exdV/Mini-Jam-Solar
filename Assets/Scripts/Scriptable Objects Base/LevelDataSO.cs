using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewLevelData",menuName = "Scriptable Object/new level data")]
public class LevelDataSO: ScriptableObject
{
    public AudioClip audioClip;
    public string midiFileLocation;

    public SceneEnum sceneLocation;
    
    public float spawnTimeAdvance;
    
}