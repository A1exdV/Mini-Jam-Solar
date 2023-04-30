using System;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class HealthProgressBarVisual : MonoBehaviour
    {
        [SerializeField] private Image healthFill;

        [SerializeField] private Image sunImage;
        
        [SerializeField] private Image cloudImage1;
        [SerializeField] private Image cloudImage2;
        [SerializeField] private Image cloudImage3;

        [SerializeField] private Color dangerColor;

        private void Start()
        {
            healthFill.fillAmount = 1f;
            NoteGameManager.Instance.onHealthChanged+= OnHealthChanged;
        }

        private void OnHealthChanged(object sender, float e)
        {
            print($"fill - {e}");
            healthFill.fillAmount = e;
        }
    }
}
