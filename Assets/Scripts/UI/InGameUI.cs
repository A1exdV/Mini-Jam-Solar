using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private List<Button> mainMenuButton;
        [SerializeField] private List<Button> restartButton;


        private void Start()
        {
            foreach (var button in mainMenuButton)
            {
                button.onClick.AddListener(OnMainMenuCall);
            }
        }

        private void OnMainMenuCall()
        {
            GameStateManager.OnChangeState(this, GameState.Exiting);
        }
    }
}
