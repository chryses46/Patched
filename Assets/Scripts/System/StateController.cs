using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum State {MainMenu, Play, Death, Pause};

    public State gameState;

    public static StateController instance;

    void Awake()
    {
        instance = this;
        gameState = State.MainMenu;
    }
}

