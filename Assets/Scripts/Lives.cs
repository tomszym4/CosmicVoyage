using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {

    //Shows HP onscreen

    private Text onScreenHP;
    public PlayerController playerController;

    private void Start()
    {
        onScreenHP = GetComponent<Text>();
        onScreenHP.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.inGame += OnGameStart;
        GameManager.lose += OnGameLose;
    }

    private void OnDisable()
    {
        GameManager.inGame -= OnGameStart;
        GameManager.lose -= OnGameLose;
    }

    public void OnGameStart()
    {
        onScreenHP.enabled = true;
        onScreenHP.text = "HP: " + playerController.playerHealth.ToString();
    }

    public void OnGameLose()
    {
        onScreenHP.enabled = false;
    }

    public void LivesUpdate(float value)
    {
        onScreenHP.text = "HP: " + playerController.playerHealth.ToString();
    }
}
