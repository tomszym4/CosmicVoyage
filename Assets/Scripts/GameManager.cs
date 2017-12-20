using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int aliveEnemies = 0;
    public static int killedEnemies = 0;

    public delegate void MainMenu();
    public delegate void Preparation();
    public delegate void InGame();
    public delegate void Pause();
    public delegate void Lose();

    public static event MainMenu mainMenu;
    public static event Preparation preparation;
    public static event InGame inGame;
    public static event Pause pause;
    public static event Lose lose;



    public void MainMenuEvent()
    {
        //waiting for enemies for leaving area after player death before allowing new game    
            if (aliveEnemies == 0)
            {
                if (mainMenu != null)
                    mainMenu();
            }
    }

    public void PreparationEvent()
    {
        if (preparation != null)
            preparation();
    }

    public void InGameEvent()
    {
        if (inGame != null)
            inGame();
    }

    public void PauseEvent()
    {
        if (pause != null)
            pause();
    }

    public void LoseEvent()
    {
        if (lose != null)
            lose();
    }
}
