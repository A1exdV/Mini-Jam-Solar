using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class EndGamePanelVisual : MonoBehaviour
    {
        [SerializeField] private float oneElementAnimTime;

        [Space] [SerializeField] private Image background;

        [Space] [SerializeField] private TextMeshProUGUI winText;
        [SerializeField] private TextMeshProUGUI loseText;
        
        [Space]
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Buttons Images")] [SerializeField]
        private Image restartButtonImage;

        [SerializeField] private Image mainMenuButtonImage;
        [SerializeField] private Image continueButtonImage;

        [Header("Buttons Test")] [SerializeField]
        private TextMeshProUGUI restartButtonText;

        [SerializeField] private TextMeshProUGUI mainMenuButtonText;
        [SerializeField] private TextMeshProUGUI continueButtonText;

        [Header("Statistics tests")] [SerializeField]
        private TextMeshProUGUI totalNotesText;

        [SerializeField] private TextMeshProUGUI notesHitText;
        [SerializeField] private TextMeshProUGUI notesMissedTest;
        [SerializeField] private TextMeshProUGUI maxMultiplierText;
        [SerializeField] private TextMeshProUGUI perfectHitsText;
        [SerializeField] private TextMeshProUGUI greatHitsText;
        [SerializeField] private TextMeshProUGUI niceHitsText;

        [Header("Statistics test outputs")] [SerializeField]
        private TextMeshProUGUI totalNotesTextOutput;

        [SerializeField] private TextMeshProUGUI notesHitTextOutput;
        [SerializeField] private TextMeshProUGUI notesMissedTestOutput;
        [SerializeField] private TextMeshProUGUI maxMultiplierTextOutput;
        [SerializeField] private TextMeshProUGUI perfectHitsTextOutput;
        [SerializeField] private TextMeshProUGUI greatHitsTextOutput;
        [SerializeField] private TextMeshProUGUI niceHitsTextOutput;


        private void Start()
        {
            GameStateManager.onGameEndedState += OnGameEnd;
            var noAlphaColor = new Color(255, 255, 255, 0);

            background.color = noAlphaColor;

            totalNotesTextOutput.color = noAlphaColor;
            notesHitTextOutput.color = noAlphaColor;
            notesMissedTestOutput.color = noAlphaColor;
            maxMultiplierTextOutput.color = noAlphaColor;
            perfectHitsTextOutput.color = noAlphaColor;
            greatHitsTextOutput.color = noAlphaColor;
            niceHitsTextOutput.color = noAlphaColor;

            totalNotesText.color = noAlphaColor;
            notesHitText.color = noAlphaColor;
            notesMissedTest.color = noAlphaColor;
            maxMultiplierText.color = noAlphaColor;
            perfectHitsText.color = noAlphaColor;
            greatHitsText.color = noAlphaColor;
            niceHitsText.color = noAlphaColor;

            restartButtonText.color = noAlphaColor;
            mainMenuButtonText.color = noAlphaColor;
            continueButtonText.color = noAlphaColor;

            winText.color = noAlphaColor;
            loseText.color = noAlphaColor;

            scoreText.color = noAlphaColor;

            restartButtonImage.color = noAlphaColor;
            mainMenuButtonImage.color = noAlphaColor;
            continueButtonImage.color = noAlphaColor;

            gameObject.SetActive(false);
        }

        private void OnGameEnd(object sender, EventArgs e)
        {
            var statistics = NoteGameManager.Instance.GetStatistics();
            gameObject.SetActive(true);

            var sequence = DOTween.Sequence();
            sequence.SetDelay(3f);
            sequence.Append(background.DOFade(1, oneElementAnimTime));
            
            sequence.Join(restartButtonImage.DOFade(1, oneElementAnimTime));
            sequence.Join(mainMenuButtonImage.DOFade(1, oneElementAnimTime));
           

            sequence.Join(restartButtonText.DOFade(1, oneElementAnimTime));
            sequence.Join(mainMenuButtonText.DOFade(1, oneElementAnimTime));

            if (statistics.isWin)
            {             
                sequence.Join(continueButtonText.DOFade(1, oneElementAnimTime));
                sequence.Join(continueButtonImage.DOFade(1, oneElementAnimTime));
                sequence.Append(winText.DOFade(1, oneElementAnimTime));
            }
            else
            {
                continueButtonImage.gameObject.SetActive(false);
                sequence.Append(loseText.DOFade(1, oneElementAnimTime));
            }
            
            sequence.Append(scoreText.DOFade(1, oneElementAnimTime));

            sequence.Append(totalNotesTextOutput.DOFade(1, oneElementAnimTime));
            sequence.Join( totalNotesText.DOFade(1, oneElementAnimTime));

            sequence.Append(notesHitText.DOFade(1, oneElementAnimTime));
            sequence.Join(notesHitTextOutput.DOFade(1, oneElementAnimTime));

            sequence.Append(notesMissedTest.DOFade(1, oneElementAnimTime));
            sequence.Join(notesMissedTestOutput.DOFade(1, oneElementAnimTime));

            sequence.Append(maxMultiplierText.DOFade(1, oneElementAnimTime));
            sequence.Join(maxMultiplierTextOutput.DOFade(1, oneElementAnimTime));

            sequence.Append(perfectHitsText.DOFade(1, oneElementAnimTime));
            sequence.Join(perfectHitsTextOutput.DOFade(1, oneElementAnimTime));

            sequence.Append(greatHitsText.DOFade(1, oneElementAnimTime));
            sequence.Join(greatHitsTextOutput.DOFade(1, oneElementAnimTime));

            sequence.Append(niceHitsText.DOFade(1, oneElementAnimTime));
            sequence.Join(niceHitsTextOutput.DOFade(1, oneElementAnimTime));

            sequence.Play();
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}