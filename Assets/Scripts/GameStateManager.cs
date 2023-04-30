using System;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStateManager : MonoBehaviour
{
    public static EventHandler onTransitionEnd;

    public static EventHandler onLoadedState;
    public static EventHandler onPlayingState;
    public static EventHandler onPausedState;
    public static EventHandler onGameEndedState;
    public static EventHandler onExitingState;
    public static EventHandler onExitState;

    public static EventHandler<GameState> OnChangeState;


    private static GameState _state;

    private void Start()
    {
        _state = GameState.Loading;
        onTransitionEnd += OnTransitionEnd;
        OnChangeState += ChangeState;
    }

    private void OnTransitionEnd(object sender, EventArgs e)
    {
        if (_state == GameState.Loaded)
        {
            OnChangeState?.Invoke(this, GameState.Playing);
            return;
        }

        if (_state == GameState.Exiting)
            OnChangeState?.Invoke(this, GameState.Exit);
    }

    private void ChangeState(object sender, GameState newState)
    {
        if (_state == newState)
            return;
        _state = newState;

        print(newState);

        switch (_state)
        {
            case GameState.Loading:
                break;
            case GameState.Loaded:
                onLoadedState?.Invoke(this, EventArgs.Empty);
                TransitionVisual.onTransitionRequired?.Invoke(this, new TransitionEventArgs
                {
                    toVisible = false,
                    transitionTime = 2,
                    needCallback = true
                });
                break;
            case GameState.Playing:
                onPlayingState?.Invoke(this, EventArgs.Empty);
                break;
            case GameState.Paused:
                onPausedState?.Invoke(this, EventArgs.Empty);
                break;
            case GameState.GameEnded:
                onGameEndedState?.Invoke(this, EventArgs.Empty);
                break;
            case GameState.Exiting:
                onExitingState?.Invoke(this, EventArgs.Empty);
                TransitionVisual.onTransitionRequired?.Invoke(this, new TransitionEventArgs
                {
                    toVisible = true,
                    transitionTime = 2,
                    needCallback = true
                });
                break;
            case GameState.Exit:
                onExitState?.Invoke(this, EventArgs.Empty);

                SceneManager.LoadScene((int)SceneEnum.Menu);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }


    public static GameState GetState()
    {
        return _state;
    }

    private void OnDestroy()
    {
        onTransitionEnd = null;

        onLoadedState = null;
        onPlayingState = null;
        onPausedState = null;
        onGameEndedState = null;
        onExitingState = null;
        onExitState = null;

        OnChangeState = null;
    }
}