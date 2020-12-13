using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //MainMenu State controls
        if(StateController.instance.gameState == StateController.State.MainMenu)
        {
            if(Input.GetButtonDown("Jump"))
            {
                UIController.instance.MainMenuUI(false);
                GameController.instance.Play();
            }  
        }

        //Play state controls
        if(StateController.instance.gameState == StateController.State.Play)
        {
            if(Input.GetButtonDown("Jump"))
            {
                player.Jump();
            }

            if(Input.GetAxis("Horizontal") > 0)
            {
                player.MoveRight();
            }
            else if(Input.GetAxis("Horizontal") < 0)
            {
                player.MoveLeft();
            }
            else if(Input.GetAxis("Horizontal") == 0)
            {
                player.Idle();
            }

            if(Input.GetAxis("Vertical") > 0)
            {
                player.MoveUp();
            }
            else if(Input.GetAxis("Vertical") < 0)
            {
                player.MoveDown();
            }

			player.DirectInput(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

            // Pause the game
            if(Input.GetButtonDown("Cancel"))
            {
                StartCoroutine(PausePress());
                
            }

            if(Debug.isDebugBuild)
            {
                //Debug Controls
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    player.GiveArms(!player.hasArms);
                }
                
                if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    player.GiveDoubleJump(!player.hasDoubleJump);
                }

                if(Input.GetKeyDown(KeyCode.K))
                {
                    UIController.instance.UpdateLife(0);
                }
            }
        }
        //Pause State Controls
        if(StateController.instance.gameState == StateController.State.Pause)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                UIController.instance.PauseUI(false);
                GameController.instance.Play();
            }
        }

        //Death state controls
        if(StateController.instance.gameState == StateController.State.Death)
        {
            if(Input.GetButtonDown("Jump"))
            {
                player.RespawnAtLastCheckpoint();
                UIController.instance.DeathUI(false);
                GameController.instance.Play();
            }
        }

        

    }

    private IEnumerator PausePress()
    {
        yield return null;
        UIController.instance.PlayUI(false);
        GameController.instance.Pause();
    }

    
}
