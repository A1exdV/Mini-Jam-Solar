using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        [SerializeField] private List<GameObject> cloudsList;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            var openLevels = SaveManager.Instance.GetSaveData<List<bool>>(SaveDataEnum.OpenLevels);

            for (var i = 1; i < openLevels.Count; i++)
            {
                if (openLevels[i])
                {
                    cloudsList[i-1].SetActive(false);
                }
            }
            
            backButton.onClick.AddListener(BackButton_OnClick);
        }

        private void BackButton_OnClick()
        {
            gameObject.SetActive(false);
        }
    }
}
