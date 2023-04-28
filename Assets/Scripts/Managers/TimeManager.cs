using System;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;

        public EventHandler onStopTime;
        public EventHandler onPlayTime;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            onStopTime += OnStopTime;
            onPlayTime += OnPlayTime;
        }

        public float GetMusicSourceTime()
        {
            return (float)musicSource.timeSamples / musicSource.clip.frequency;
        }

        private void OnPlayTime(object sender, EventArgs e)
        {
            Time.timeScale = 1;
        }

        private void OnStopTime(object sender, EventArgs e)
        {
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }

        public float TimeBeforeNoteByIndex(int index)
        {
            return LevelDataManager.Instance.GetTimeStampByIndex(index) - GetMusicSourceTime();
        }
    }
}