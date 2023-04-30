using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueButton;

    [SerializeField] private TextMeshProUGUI totalNotesText;
    [SerializeField] private TextMeshProUGUI notesHitText;
    [SerializeField] private TextMeshProUGUI notesMissedTest;
    [SerializeField] private TextMeshProUGUI maxMultiplierText;
    [SerializeField] private TextMeshProUGUI perfectHitsText;
    [SerializeField] private TextMeshProUGUI greatHitsText;
    [SerializeField] private TextMeshProUGUI niceHitsText;

    private void Start()
    {
        NoteGameManager.Instance.onGameEnd+= OnGameEnd;
    }

    private void OnGameEnd(object sender, Statistics e)
    {
        totalNotesText.text = e.totalNotes.ToString();
        notesHitText.text = e.totalNotesHit.ToString();
        notesMissedTest.text =e.totalNotesMissed.ToString();
        maxMultiplierText.text =e.maxMultiplier.ToString();
        perfectHitsText.text = e.totalPerfectHits.ToString();
        greatHitsText.text = e.totalGreatHits.ToString();
        niceHitsText.text = e.totalNiceHits.ToString();
    }
}
