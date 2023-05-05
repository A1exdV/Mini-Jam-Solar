using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalNotesText;
    [SerializeField] private TextMeshProUGUI notesHitText;
    [SerializeField] private TextMeshProUGUI notesMissedTest;
    [SerializeField] private TextMeshProUGUI maxMultiplierText;
    [SerializeField] private TextMeshProUGUI perfectHitsText;
    [SerializeField] private TextMeshProUGUI greatHitsText;
    [SerializeField] private TextMeshProUGUI niceHitsText;

    private void Start()
    {
        GameStateManager.onGameEndedState += OnGameEnd;
    }

    private void OnGameEnd(object sender, EventArgs e)
    {
        var statistics = NoteGameManager.Instance.GetStatistics();
        scoreText.text = statistics.score.ToString();
        totalNotesText.text = statistics.totalNotes.ToString();
        notesHitText.text = statistics.totalNotesHit.ToString();
        notesMissedTest.text =statistics.totalNotesMissed.ToString();
        maxMultiplierText.text =statistics.maxMultiplier.ToString();
        perfectHitsText.text = statistics.totalPerfectHits.ToString();
        greatHitsText.text = statistics.totalGreatHits.ToString();
        niceHitsText.text = statistics.totalNiceHits.ToString();
    }
}
