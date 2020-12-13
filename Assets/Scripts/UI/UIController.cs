using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] Canvas MainMenuUICanvas;
    [SerializeField] Canvas GamePlayUICanvas;
    [SerializeField] Canvas PauseScreenUICanvas;
    [SerializeField] Canvas DeathScreenUICanvas;
    [SerializeField] LifeBar lifeBar;

    public static UIController instance;

    void Awake()
    {
        instance = this;
    }
    
    // Value can be 1 or < 1 (-1)
    public void UpdateLife(int value)
    {
        if(value == 0)
        {
            GameController.instance.Death();
            PlayUI(false);
        }
        else
        {
            lifeBar.life = value;
        }    
    }

    public void MainMenuUI(bool isEnabled)
    {
        MainMenuUICanvas.gameObject.SetActive(isEnabled);
    }

    public void PlayUI(bool isEnabled)
    {
        GamePlayUICanvas.gameObject.SetActive(isEnabled);
    }

    public void PauseUI(bool isEnabled)
    {
        PauseScreenUICanvas.gameObject.SetActive(isEnabled);
    }

    public void DeathUI(bool isEnabled)
    {
        PlayUI(false);
        DeathScreenUICanvas.gameObject.SetActive(isEnabled);
    }
}
