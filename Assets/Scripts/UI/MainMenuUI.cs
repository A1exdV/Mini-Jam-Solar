using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private List<Button> backToMainMenuButtonsList;

        [Header("UI holders")] 
        [SerializeField] private GameObject levelSelectionUI;
        [SerializeField] private GameObject settingsUI;
        [SerializeField] private GameObject creditsUI;

        [Header("UI Groups")] 
        [SerializeField] private GameObject transitionObject;

        private void Awake()
        {
            gameObject.SetActive(true);
        }

        private void Start()
        {
            playButton.onClick.AddListener(PlayButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
            creditsButton.onClick.AddListener(CreditsButton_OnClick);
            exitButton.onClick.AddListener(ExitButton_OnClick);

            foreach (var button in backToMainMenuButtonsList)
            {
                button.onClick.AddListener(BackToMainMenuButton_OnClick);
            }
            transitionObject.GetComponent<TransitionVisual>().RunTransition(new TransitionEventArgs
            {
                toVisible = false,
                transitionTime = 2,
                needCallback = false
            });
        }

        private void PlayButton_OnClick()
        {
            gameObject.SetActive(false);
            levelSelectionUI.SetActive(true);
        }

        private void SettingsButton_OnClick()
        {
            gameObject.SetActive(false);
            settingsUI.SetActive(true);
        }
        
        private void CreditsButton_OnClick()
        {
            gameObject.SetActive(false);
            creditsUI.SetActive(true);
        }

        private void ExitButton_OnClick()
        {
            Debug.Log("Game closed by player.");
            Application.Quit();
        }

        private void BackToMainMenuButton_OnClick()
        {
            gameObject.SetActive(true);
        }
    }
}
