using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private float countdownTimer;

    [SerializeField] private TextMeshProUGUI countdownText;

    private bool _countdownStarted = false;

    private void Awake()
    {
        countdownText.enabled = false;
    }

    private void Update()
    {
        if (_countdownStarted)
            return;

        if (NoteGameManager.Instance.GetMusicSourceTime() >=
            NoteGameManager.Instance.GetFirstNoteSpawnAdvanceTime() - countdownTimer)
        {
            countdownText.enabled = true;
            StartCoroutine(CountdownRoutine());
        }
    }

    private IEnumerator CountdownRoutine()
    {
        var timer = countdownTimer;
        
        var timerInt = Mathf.CeilToInt(timer);

        var lastNumber = timerInt;
        
        countdownText.text = timerInt.ToString();
        
        while (timer <= 0)
        {
            timer -= Time.deltaTime;
            
            if (lastNumber != timerInt)
            {
                countdownText.text = timerInt.ToString();
                lastNumber = timerInt;
            }
            
            yield return null;
        }
        
        countdownText.enabled = false;
        yield break;
    }
}