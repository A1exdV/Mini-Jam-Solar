using UnityEngine;
using UnityEngine.SceneManagement;


public class GameInitializationManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    
    private LevelDataSO _levelDataSo;
    
    private void Start()
    {
        _levelDataSo = LevelDataHolder.Instance.GetLevelDataSO();
        
         NoteGameManager.Instance.Initialization(musicSource);
         
        SceneManager.LoadScene((int)LevelDataHolder.Instance.GetLevelDataSO().sceneLocation, LoadSceneMode.Additive);
        
        musicSource.clip = _levelDataSo.audioClip;
        musicSource.Play();
    }
}