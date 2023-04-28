using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StarVisual : MonoBehaviour
{
    [SerializeField] private GameObject visual;

    private int _index;
    private NoteManager _noteManager;
    private float _maxAngle = 80f;

    private float _startAngle;
    private float _endAngle;
    

    private void Awake()
    {
        visual.SetActive(false);
    }

    private void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            _startAngle = _maxAngle;
            _endAngle = -_maxAngle;
        }
        else
        {
            _startAngle = -_maxAngle;
            _endAngle = _maxAngle;
        }
        _noteManager.onNewNoteDraw += OnNewNoteDraw;
        transform.localEulerAngles = new Vector3(0,0 ,_startAngle);
        
    }

    private void OnNewNoteDraw(object sender, NoteManager.EventArgsOnNewNoteDraw eventArgsOnNewNoteDraw)
    {
        if (_index == eventArgsOnNewNoteDraw.index)
        {
            print(eventArgsOnNewNoteDraw.lifetime);
            StartMoving(eventArgsOnNewNoteDraw.lifetime);
        }
    }

    public void Initialisation(int index, NoteManager noteManager)
    {
        _index = index;
        _noteManager = noteManager;
    }

    public void StartMoving(float lifeTime)
    {
        print("moving");
        visual.SetActive(true);
        transform.DOLocalRotate(new Vector3(0, 0, _endAngle), lifeTime*2,RotateMode.LocalAxisAdd);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
