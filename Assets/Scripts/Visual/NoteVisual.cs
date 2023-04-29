using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class NoteVisual : MonoBehaviour
    {
        private NoteGameManager.NoteData _noteData;
        private Image _image;

        private float _endAngle = 98f;
        public void SetNoteData(NoteGameManager.NoteData noteData)
        {
            _noteData = noteData;
        }

        private void Start()
        {
            _noteData.onNoteMakeVisible += OnNoteMakeVisible;
            _image = GetComponentInChildren<Image>();
            _image.color = new Color(1,1,1,0);
            _noteData.onNoteDestroy+= OnNoteDestroy;
            gameObject.SetActive(false);
        }

        private void OnNoteDestroy(object sender, EventArgs e)
        {
            DOTween.Kill(transform);
            DOTween.Kill(_image);
            Destroy(gameObject);
            
        }

        private void OnNoteMakeVisible(object sender, double e)
        {
            gameObject.SetActive(true);
            var time = (float)e;
            print(time);
            transform.localEulerAngles = new Vector3(0, 0, -89f);
            /*var sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalRotate(new Vector3(0,0,89f), time, RotateMode.LocalAxisAdd));
            sequence.Append(transform.DOLocalRotate(new Vector3(0,0,89f), time, RotateMode.LocalAxisAdd));
            sequence.Play();
            sequence.OnComplete(() =>
            {
                print("Done!");
            });*/
            StartCoroutine(Rotation(time));
            _image.DOFade(1, time/2);
        }

        private IEnumerator Rotation(float time)
        {
            var timer = 0f;
            while (transform.localEulerAngles.y != _endAngle)
            {
                timer += Time.deltaTime;
                var currentAngle = Mathf.LerpAngle(-89f, 89,timer/(time*2));
                transform.localEulerAngles = new Vector3(0, 0, currentAngle);
                yield return null;
            }
        }
    }
}
