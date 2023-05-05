using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> slides;
    
    [SerializeField] private Button backButton; 
    [SerializeField] private Button nextButton;
    
    [SerializeField] private Button firstLevelButton;
    
    [SerializeField] private LevelDataSO levelDataSo;
    
    private int _index;
    private TextMeshProUGUI _nextButtonText;

    private void Start()
    {
        _nextButtonText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        
        backButton.onClick.AddListener(OnBackButtonCall);
        nextButton.onClick.AddListener(OnNextButtonCall);
        firstLevelButton.onClick.AddListener(OnFirstLevelButtonCall);
        
        gameObject.SetActive(false);
    }

    private void OnFirstLevelButtonCall()
    {
        gameObject.SetActive(true);
        _index = 0;
        ChangeSlidesVisibility();
    }

    private void ChangeSlidesVisibility()
    {
        for (var i = 0; i < slides.Count; i++)
        {
            slides[i].SetActive(_index == i);
        }

        if (_index == slides.Count - 1)
        {
            _nextButtonText.text = "Start!";
        }
        else
        {
            _nextButtonText.text = "Next";
        }
    }

    private void OnNextButtonCall()
    {
        _index++;
        if (_index >= slides.Count)
        {
            if (SaveManager.Instance.IsLevelOpen(levelDataSo.index))
                TransitionVisual.onTransitionRequired?.Invoke(this, new TransitionEventArgs
                {
                    toVisible = true,
                    transitionTime = 1,
                    callback = () => { LevelDataHolder.Instance.LoadNewLevelData(levelDataSo); }
                });
        }
        else
        {
            ChangeSlidesVisibility();
        }
    }

    private void OnBackButtonCall()
    {
        _index--;
        if (_index < 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            ChangeSlidesVisibility();
        }
    }
}
