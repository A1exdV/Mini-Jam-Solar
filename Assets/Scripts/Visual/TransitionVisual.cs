using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransitionEventArgs : EventArgs
{
    public bool toVisible;
    public float transitionTime = 0;
    public Action callback = null;
}

public class TransitionVisual : MonoBehaviour
{
    [SerializeField] private float transitionTime = 2f;
    private RectTransform _rectTransform;
    private bool _isVisible;

    public static EventHandler<TransitionEventArgs> onTransitionRequired;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        onTransitionRequired += OnTransitionRequired;
        _isVisible = true;
    }

    public void RunTransition(TransitionEventArgs e)
    {
        OnTransitionRequired(null, e);
    }

    private void OnTransitionRequired(object sender, TransitionEventArgs e)
    {
        print("transition required");
        if (_isVisible == e.toVisible)
        {
            Debug.LogError($"Transition direction error! Sender - {sender}");
            return;
        }
        if (e.transitionTime != 0)
        {
            transitionTime = e.transitionTime;
        }


        var sequence = DOTween.Sequence();

        if (e.toVisible)
        {
            _rectTransform.localPosition = new Vector3(0, -1080, 0);
            sequence.Append(_rectTransform.DOLocalMoveY(0, transitionTime));
            _isVisible = true;
        }
        else
        {
            sequence.SetDelay(1f);
            _rectTransform.localPosition = new Vector3(0, 0, 0);
            sequence.Append(_rectTransform.DOLocalMoveY(-1080f, transitionTime));
            _isVisible = false;
        }

        sequence.Play();
        StartCoroutine(TweenEnded(e.callback, sequence));
    }

    private IEnumerator TweenEnded(Action callback, Sequence sequence)
    {
        while (sequence.IsActive())
        {
            yield return null;
        }
        callback?.Invoke();
        print("transition ended");

        yield break;
    }

    private void OnDisable()
    {
        DOTween.Kill(_rectTransform);
        onTransitionRequired -= onTransitionRequired;
    }
}