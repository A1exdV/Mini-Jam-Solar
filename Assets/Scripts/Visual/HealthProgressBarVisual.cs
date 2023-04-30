using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class HealthProgressBarVisual : MonoBehaviour
    {
        [SerializeField] private Image healthFill;

        [SerializeField] private Image sunImage;
        
        [SerializeField] private List<Image> cloudImageList;


        [SerializeField] private Color dangerColor;

        private int _triggeredClouds;

        private void Start()
        {
            healthFill.fillAmount = 1f;
            NoteGameManager.Instance.onHealthChanged+= OnHealthChanged;
            NoteGameManager.Instance.onCloudTriggered+= OnCloudTriggered;
        }

        private void OnCloudTriggered(object sender, EventArgs e)
        {
            cloudImageList[_triggeredClouds].enabled = false;
            _triggeredClouds++;
        }

        private void OnHealthChanged(object sender, float e)
        {
            healthFill.fillAmount = e;
        }
    }
}
