using System;
using System.Collections;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField] private float maxCountdownTimer = 3;
        [SerializeField] private TextMeshProUGUI countdownText;
        
        private float _timer;
        private float _noteSpawnAdvance;
        private void Awake()
        {
            countdownText.gameObject.SetActive(false);
        }

        private void Start()
        {
            _noteSpawnAdvance = TimeManager.Instance.GetNoteSpawnAdvance();
            GameStateManager.onStateCountdown +=  GameStateManager_OnStateCountdown;
        }

        private void  GameStateManager_OnStateCountdown(object sender, EventArgs e)
        {
            if (GameStateManager.Instance.GetPreviousState() == GameStateManager.State.Paused)
            {
                return;
            }

            if (TimeManager.Instance.TimeBeforeNoteByIndex(0)-_noteSpawnAdvance < maxCountdownTimer)
            {
                print($"{TimeManager.Instance.TimeBeforeNoteByIndex(0)}");
                maxCountdownTimer = TimeManager.Instance.TimeBeforeNoteByIndex(0);
            }

            _timer = maxCountdownTimer;
        }


        private void Update()
        {
            if (TimeManager.Instance.TimeBeforeNoteByIndex(0)-_noteSpawnAdvance > _timer)
            {
                return;
            }
            countdownText.gameObject.SetActive(true);

            if (_timer <= 0)
            {
                if (countdownText.gameObject.activeSelf)
                {
                    countdownText.gameObject.SetActive(false);
                    GameStateManager.onStateChanged?.Invoke(this, GameStateManager.State.Playing);
                }
            }
            else
            {
                int countdownNumber = Mathf.CeilToInt(_timer);
                _timer -= Time.deltaTime;
                countdownText.text = countdownNumber.ToString();
            }
        }
    }
}
