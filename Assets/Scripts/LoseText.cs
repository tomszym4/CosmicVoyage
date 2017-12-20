using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseText : MonoBehaviour {

    private Text loseText;


    private void Start()
    {
        loseText = GetComponent<Text>();
        loseText.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.mainMenu += Disabling;
        GameManager.lose += OnLoseText;
    }
    private void OnDisable()
    {
        GameManager.mainMenu -= Disabling;
        GameManager.lose -= OnLoseText;
    }

    private void Disabling()
    {
        loseText.enabled = false;
    }

    private void OnLoseText ()
    {
        loseText.enabled = true;
        loseText.text = "YOUR SCORE\n" + ScoreKeeper.points.ToString() + "\nKILLED ENEMIES\n" + GameManager.killedEnemies.ToString();
    }
}
