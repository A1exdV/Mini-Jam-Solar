using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visual
{
    public class ShamanVisual : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string missTrigger = "Miss";
        [SerializeField] private string winTrigger = "GameWin";
        [SerializeField] private string loseTrigger = "GameLose";
        
        [SerializeField] private List<string> hitTrigger;

        private void Start()
        {
            NoteGameManager.Instance.onPlayerMiss += OnPlayerMiss;
            NoteGameManager.Instance.onPlayerHit +=OnPlayerHit;
            GameStateManager.onGameEndedState += OnGameEndedState;
        }

        private void OnGameEndedState(object sender, EventArgs e)
        {
            if (NoteGameManager.Instance.GetStatistics().isWin)
            {
                animator.SetTrigger(winTrigger);
            }
            else
            {
                animator.SetTrigger(loseTrigger);
            }
        }

        private void OnPlayerHit(object sender, EventArgs e)
        {
            animator.SetTrigger(hitTrigger[Random.Range(0,hitTrigger.Count)]);
        }

        private void OnPlayerMiss(object sender, EventArgs e)
        {
            animator.SetTrigger(missTrigger);
        }
    }
}
