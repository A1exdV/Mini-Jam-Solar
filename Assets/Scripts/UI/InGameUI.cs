using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private List<Button> pauseResumeButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private List<Button> mainMenuButton;
        [SerializeField] private List<Button> restartButton;

        [SerializeField] private GameObject pauseUI;
        [SerializeField] private GameObject musicSettingsUI;
        [SerializeField] private GameObject gameCompletedUI;
        

        private void Start()
        {
            foreach (var button in mainMenuButton)
            {
                button.onClick.AddListener(OnMainMenuCall);
            }
            
            foreach (var button in pauseResumeButton)
            {
                button.onClick.AddListener(OnPauseResumeCall);
            }
            
            foreach (var button in restartButton)
            {
                button.onClick.AddListener(OnRestartCall);
            }

            musicButton.onClick.AddListener(OnMusicButtonCall);
            
            nextLevelButton.onClick.AddListener(OnNextLevelCall);
            
            InputManager.Instance.OnPauseAction += OnEscKeyPressed;
            
            pauseUI.SetActive(false);
            musicSettingsUI.SetActive(false);
        }

        private void OnNextLevelCall()
        {
            if (LevelDataHolder.Instance.GetLevelDataSO().nextLevelDataSO == null)
            {
                gameCompletedUI.SetActive(true);
                return;
            }
            TransitionVisual.onTransitionRequired?.Invoke(this,new TransitionEventArgs
            {
                toVisible = true,
                transitionTime = 1,
                callback = () =>
                {
                    LevelDataHolder.Instance.LoadNewLevelData(LevelDataHolder.Instance.GetLevelDataSO().nextLevelDataSO);
                }
            });
        }

        private void OnEscKeyPressed(object sender, EventArgs e)
        {
            OnPauseResumeCall();
        }
        private void OnRestartCall()
        {
            TransitionVisual.onTransitionRequired?.Invoke(this,new TransitionEventArgs
            {
                toVisible = true,
                transitionTime = 1,
                callback = () =>
                {
                    SceneManager.LoadScene((int)SceneEnum.Game);
                }
            });
            
        }
        private void OnMusicButtonCall()
        {
            if (GameStateManager.GetState() == GameState.Playing)
            {
                PauseUnpauseGame();
                musicSettingsUI.SetActive(true);
            }
        }

        private void OnPauseResumeCall()
        {
             pauseUI.SetActive(true);
             PauseUnpauseGame();
        }

        private void PauseUnpauseGame()
        {
            if (GameStateManager.GetState() == GameState.Playing)
            {
                GameStateManager.OnChangeState(this, GameState.Paused);
                return;
            }
            
            if (GameStateManager.GetState() == GameState.Paused)
            {
                pauseUI.SetActive(false);
                musicSettingsUI.SetActive(false);
                GameStateManager.OnChangeState(this, GameState.Playing);
            }
        }

        private void OnMainMenuCall()
        {
            GameStateManager.OnChangeState(this, GameState.Exiting);
        }
    }
}
