using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MusicManager : MonoBehaviour
{
        [SerializeField] private AudioSource musicSource;
        
        [SerializeField] private AudioSource rainSource;

        [SerializeField] private List<AudioClip> uiSfxList;

        [SerializeField] private List<AudioClip> missSoundList;
        
        [SerializeField] private List<AudioClip> thunderSoundList;

        [SerializeField] private AudioClip winSound;
        
        [SerializeField] private AudioClip loseSound;
        

        private float _musicVolume;
        private float _sfxVolume;



        private void Start()
        {
                foreach (var button in Resources.FindObjectsOfTypeAll<Button>())
                {
                        button.onClick.AddListener(OnUICall);
                }
                
                _musicVolume = SaveManager.Instance.GetSaveData<float>(SaveDataEnum.MusicVolume);
                _sfxVolume = SaveManager.Instance.GetSaveData<float>(SaveDataEnum.SfxVolume);
                print((_musicVolume,_sfxVolume));
                musicSource.volume = _musicVolume;
                if (NoteGameManager.Instance != null)
                {
                        NoteGameManager.Instance.onPlayerMiss+= OnPlayerMiss;
                        NoteGameManager.Instance.onCloudTriggered+= OnCloudTriggered;
                }
                if(rainSource!=null)
                        rainSource.volume = _sfxVolume;
        }

        private void OnCloudTriggered(object sender, EventArgs e)
        {
                print("Thunder!");
                AudioSource.PlayClipAtPoint(thunderSoundList[Random.Range(0, thunderSoundList.Count)],Vector3.zero,_sfxVolume);
        }

        private void OnUICall()
        {
                AudioSource.PlayClipAtPoint(uiSfxList[Random.Range(0, uiSfxList.Count)],Vector3.zero,_sfxVolume);
        }
        private void OnPlayerMiss(object sender, GameInfo e)
        {
                AudioSource.PlayClipAtPoint(missSoundList[Random.Range(0, missSoundList.Count)],Vector3.zero,_sfxVolume);
        }

        public float GetSfxVolume()
        {
                return _sfxVolume;
        }

        public void SetSfxVolume(float newSfx)
        {
                _sfxVolume = newSfx;
                SaveManager.Instance.SetSfxVolume(_sfxVolume);
        }
        
        public float GetMusicVolume()
        {
                return _musicVolume;
        }

        public void SetMusicVolume(float newMusic)
        {
                _musicVolume = newMusic;
                musicSource.volume = _musicVolume;
                SaveManager.Instance.SetMusicVolume(_musicVolume);
        }
}
