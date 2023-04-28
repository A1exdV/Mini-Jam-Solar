using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Visual
{
    public class NoteVisual : MonoBehaviour
    {
        private NoteGameManager.NoteData _noteData;
        private Image _image;
        public void SetNoteData(NoteGameManager.NoteData noteData)
        {
            _noteData = noteData;
        }

        private void Start()
        {
            _noteData.onNoteMakeVisible += OnNoteMakeVisible;
            _image = GetComponent<Image>();
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
            transform.DOLocalRotate(new Vector3(0,0,179f), (float)e, RotateMode.LocalAxisAdd);
            _image.DOFade(1, (float)e);
        }
    }
}
