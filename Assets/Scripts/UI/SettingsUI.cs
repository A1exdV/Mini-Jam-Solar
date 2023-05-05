using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button deleteSaveButton;
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;
        

        [SerializeField] private MusicManager musicManager;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            sfxSlider.value = musicManager.GetSfxVolume();
            musicSlider.value = musicManager.GetMusicVolume();

            deleteSaveButton.onClick.AddListener(OnDeleteSaveCall);
            backButton.onClick.AddListener(BackButton_OnClick);
            musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);

        }

        private void OnDeleteSaveCall()
        {
            SaveManager.Instance.DeleteData();
        }

        private void OnSfxSliderValueChanged(float arg0)
        {
            musicManager.SetSfxVolume(arg0);
        }

        private void OnMusicSliderValueChanged(float arg0)
        {
            musicManager.SetMusicVolume(arg0);
        }

        private void BackButton_OnClick()
        {
            gameObject.SetActive(false);
        }
    }
}
