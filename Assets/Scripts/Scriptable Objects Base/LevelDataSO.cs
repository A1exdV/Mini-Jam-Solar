using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewLevelData",menuName = "Scriptable Object/new level data")]
public class LevelDataSO: ScriptableObject
{
    public AudioClip audioClip;
    public string midiFileLocation;
}