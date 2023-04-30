using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Visual
{
    public class NoteVisual : MonoBehaviour
    {
        private NoteData _noteData;
        private Image _image;

        private float _maxAngle = 89f;

        private float _startAngle;
        private float _endAngle;
        public void SetNoteData(NoteData noteData)
        {
            _noteData = noteData;
        }

        private void Start()
        {
            _noteData.onNoteMakeVisible += OnNoteMakeVisible;
            NoteGameManager.Instance.onGameEnd += OnGameEnd;
            _image = GetComponentInChildren<Image>();
            _image.color = new Color(1,1,1,0);
            _noteData.onNoteDestroy+= OnNoteDestroy;
            gameObject.SetActive(false);
            
            if (Random.Range(0, 2) == 1)
            {
                _startAngle = _maxAngle;
                _endAngle = -_maxAngle;
            }
            else
            {
                _startAngle = -_maxAngle;
                _endAngle = _maxAngle;
            }
            
        }

        private void OnGameEnd(object sender, Statistics e)
        {
            DOTween.Kill(transform);
            DOTween.Kill(_image);
            StopAllCoroutines();
        }

        private void OnNoteDestroy(object sender, EventArgs e)
        {
            DOTween.Kill(transform);
            DOTween.Kill(_image);
            _noteData.onNoteMakeVisible -= OnNoteMakeVisible;
            NoteGameManager.Instance.onGameEnd -= OnGameEnd;
            Destroy(gameObject);
            
        }

        private void OnNoteMakeVisible(object sender, double e)
        {
            gameObject.SetActive(true);
            var time = (float)e;
            print(time);
            transform.localEulerAngles = new Vector3(0, 0, _startAngle);
            StartCoroutine(Rotation(time));
            _image.DOFade(1, time/2);
        }

        private IEnumerator Rotation(float time)
        {
            var timer = 0f;
            while (transform.localEulerAngles.y != _endAngle)
            {
                timer += Time.deltaTime;
                var currentAngle = Mathf.LerpAngle(_startAngle, _endAngle,timer/(time*2));
                transform.localEulerAngles = new Vector3(0, 0, currentAngle);
                yield return null;
            }
        }
    }
}
