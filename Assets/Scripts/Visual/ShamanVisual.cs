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
        [SerializeField] private List<string> hitTrigger;

        private void Start()
        {
            NoteGameManager.Instance.onPlayerMiss += OnPlayerMiss;
            NoteGameManager.Instance.onPlayerHit +=OnPlayerHit;
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
