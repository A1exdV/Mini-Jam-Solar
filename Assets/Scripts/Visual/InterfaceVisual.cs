using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class InterfaceVisual : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Image progressFill;


        private void Start()
        {
            NoteGameManager.Instance.onPlayerMiss+= OnPlayerMiss;
            NoteGameManager.Instance.onPlayerHit+= OnPlayerHit;
            multiplierText.text = $"x0";
            scoreText.text = $"0";
            progressFill.fillAmount = 0.85f;
        }

        private void OnPlayerHit(object sender, GameInfo e)
        {
            multiplierText.text = $"x{e.multiplier}";
            scoreText.text = $"{e.score}";
            progressFill.fillAmount = e.healthNormalized;
        }

        private void OnPlayerMiss(object sender, EventArgs e)
        {
            multiplierText.text = "x0";
        }
    }
}
