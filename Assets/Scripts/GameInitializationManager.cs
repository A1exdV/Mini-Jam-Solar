using System;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameInitializationManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    
    private LevelDataSO _levelDataSo;
    
    private void Start()
    {
        print("starting Initialization");
        GameStateManager.onPlayingState += OnPlayingState;
        GameStateManager.onPausedState += OnPausedState;
        
        _levelDataSo = LevelDataHolder.Instance.GetLevelDataSO();
        print("level data set");
        musicSource.clip = _levelDataSo.audioClip;
        
         
        SceneManager.LoadScene((int)LevelDataHolder.Instance.GetLevelDataSO().sceneLocation, LoadSceneMode.Additive);
        print("scene decoration loaded");
        
        
        NoteGameManager.Instance.Initialization(musicSource);
        GameStateManager.OnChangeState.Invoke(this, GameState.Loaded);
        
        print("initialization completed");
    }

    private void OnPausedState(object sender, EventArgs e)
    {
        musicSource.Pause();
    }

    private void OnPlayingState(object sender, EventArgs e)
    {
        musicSource.Play();
        print(musicSource.clip.name);
    }
}