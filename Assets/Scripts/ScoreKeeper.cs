using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

    public static int points = 0;
    private Text onScreenScore;

    private void Start()
    {
        onScreenScore = GetComponent<Text>();
        onScreenScore.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.preparation += Preparation;
        GameManager.inGame += OnGameStart;
        GameManager.lose += OnGameLose;
    }

    private void OnDisable()
    {
        GameManager.preparation -= Preparation;
        GameManager.inGame -= OnGameStart;
        GameManager.lose -= OnGameLose;
    }

    public void Preparation()
    {
        points = 0;
    }

    public void OnGameStart()
    {
        onScreenScore.text = "Score " + points.ToString();
        onScreenScore.enabled = true;
    }

    public void OnGameLose()
    {
        onScreenScore.enabled = false;
    }

    public void Score(int value)
    {
        points = points + value;
        onScreenScore.text = "Score " + points.ToString();
    }
}
