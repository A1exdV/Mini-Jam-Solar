using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            backButton.onClick.AddListener(BackButton_OnClick);
        }

        private void BackButton_OnClick()
        {
            gameObject.SetActive(false);
        }
    }
}
