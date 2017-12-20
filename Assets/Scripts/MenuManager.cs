using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {


    public enum gameState { MainMenu, Preparation, InGame, Lose, Pause };
    public gameState myState = gameState.MainMenu;

    public GameObject mainMenu;
    public GameObject inGameMenu;
    public GameObject pauseMenu;
    public GameObject loseMenu;

    private void OnEnable()
    {
        GameManager.mainMenu += BackToMain;
        GameManager.inGame += InGame;
        GameManager.pause += Pause;
        GameManager.lose += Lose;
        GameManager.preparation += StartGame;
    }

    private void OnDisable()
    {
        GameManager.mainMenu -= BackToMain;
        GameManager.inGame -= InGame;
        GameManager.pause -= Pause;
        GameManager.lose -= Lose;
        GameManager.preparation -= StartGame;
    }

    public void Start()
    {
        SwitchMenu(gameState.MainMenu);
    }

    public void StartGame()
    {
        SwitchMenu(gameState.Preparation);
        myState = gameState.Preparation;
    }

    public void InGame()
    {
        SwitchMenu(gameState.InGame);
        myState = gameState.InGame;
    }

    public void Pause()
    {
        SwitchMenu(gameState.Pause);
        myState = gameState.Pause;
    }

    public void Lose()
    {
        SwitchMenu(gameState.Lose);
        myState = gameState.Lose;
    }

    public void BackToMain()
    {
        SwitchMenu(gameState.MainMenu);
        myState = gameState.MainMenu;
    }

    public void SwitchMenu (gameState myState)
    {

        switch (myState)
        {
            case gameState.MainMenu:
                mainMenu.SetActive(true);
                loseMenu.SetActive(false);
                break;
            case gameState.Preparation:
                mainMenu.SetActive(false);
                break;
            case gameState.InGame:
                inGameMenu.SetActive(true);
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                break;
            case gameState.Pause:
                inGameMenu.SetActive(false);
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                break;
            case gameState.Lose:
                myState = gameState.Lose;
                loseMenu.SetActive(true);
                inGameMenu.SetActive(false);
                break;
            default:
                break;
        }

    }

}
