using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class CloudsVisual : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    private List<Transform> _cloudList;

    private int _index = 1;

    private float _progressForOneCloud;
    private void Awake()
    {
        _cloudList = new List<Transform>();
        foreach (Transform child in transform)
        {
            _cloudList.Add(child);
        }
        _progressForOneCloud = 1f / _cloudList.Count;
    }

    private void Update()
    {
        var timeNormalised = musicSource.time / musicSource.clip.length;

        if (timeNormalised >= _progressForOneCloud * _index)
        {
            var dir = Random.Range(0,2);
            _cloudList[_index - 1].DOLocalMoveX(dir == 0? 10:-10,3f);
            _index++;
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(_cloudList);
    }
}
