using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    public int controllers = 0;

    void Awake()
    {
        Time.timeScale = 0;
        instance = this;
    }

    void Update()
    {
        CheckForControllers();
    }

    private void CheckForControllers()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected and use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void Play()
    {
        StateController.instance.gameState = StateController.State.Play;
        ControllerVibration(0,0);
        UIController.instance.PlayUI(true);
        AudioController.instance.StartMusic();
        SetTimeScale(1);
    }

    public void Pause()
    {
        StateController.instance.gameState = StateController.State.Pause;
        UIController.instance.PauseUI(true);
        SetTimeScale(0);
    }

    public void Death()
    {
        StateController.instance.gameState = StateController.State.Death;
        ControllerVibration(1, 1);
        UIController.instance.DeathUI(true);
        SetTimeScale(0);
    }

    public void ControllerVibration(float leftMotor, float rightMotor)
    {
        GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
    }
    
}
