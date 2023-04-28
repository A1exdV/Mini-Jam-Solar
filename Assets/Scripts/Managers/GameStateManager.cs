using System;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public enum State
    {
        Loading,
        Countdown,
        Playing,
        Paused,
        Win,
        Lose
    }

    public static EventHandler<State> onStateChanged;

    public static EventHandler onStateLoading;
    public static EventHandler onStateCountdown;
    public static EventHandler onStatePlaying;
    public static EventHandler onStatePaused;
    public static EventHandler onStateWin;
    public static EventHandler onStateLose;


    private State _currentState;
    private State _previousState;

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
        
        _currentState = State.Loading;
        _previousState = _currentState;
    }

    private void Start()
    {
        onStateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, State e)
    {
        if (e == _currentState)
        {
            return;
        }

        _previousState = _currentState;
        _currentState = e;
        
        switch (_currentState)
        {
            case State.Loading:
                TimeManager.Instance.onPlayTime?.Invoke(this, EventArgs.Empty);
                onStateLoading?.Invoke(this, EventArgs.Empty);
                break;
            case State.Countdown:
                TimeManager.Instance.onPlayTime?.Invoke(this, EventArgs.Empty);
                onStateCountdown?.Invoke(this, EventArgs.Empty);
                break;
            case State.Playing:
                TimeManager.Instance.onPlayTime?.Invoke(this, EventArgs.Empty);
                onStatePlaying?.Invoke(this, EventArgs.Empty);
                break;
            case State.Paused:
                TimeManager.Instance.onStopTime?.Invoke(this, EventArgs.Empty);
                onStatePaused?.Invoke(this, EventArgs.Empty);
                break;
            case State.Win:
                onStateWin?.Invoke(this, EventArgs.Empty);
                break;
            case State.Lose:
                onStateLose?.Invoke(this, EventArgs.Empty);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(e), e, null);
        }
    }

    public State GetCurrentState()
    {
        return _currentState;
    }

    public State GetPreviousState()
    {
        return _previousState;
    }
}