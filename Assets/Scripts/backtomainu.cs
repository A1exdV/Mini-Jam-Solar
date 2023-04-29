using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class backtomainu : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    private void Update()
    {
        text.text = $"Score: {NoteGameManager.score}, Hits in a row: {NoteGameManager.hitInARow}";
    }
}
