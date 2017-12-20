using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {

    Text text;
    Animator anim;

    private int countdownTime = 3;
    private int counted;
    private float timer;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        text = GetComponent<Text>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.preparation += StartCounting;
    }
    private void OnDisable()
    {
        GameManager.preparation -= StartCounting;
    }



    private void StartCounting()
    {
        anim.enabled = true;
        text.enabled = true;
        counted = countdownTime;
        for (int i = 0; i <= counted+1; i++)
        {
            Invoke("Counting", i);
        }
    }


    void Counting()
    {
        if (counted == 0)
        {
            text.text = "Battle!";
            StartCoroutine(Disabling());
        }else
        {
            text.text = counted.ToString();
        }
        anim.Play("Countdown");
        counted--; 
    }

    IEnumerator Disabling()
    {
        yield return new WaitForSeconds(1f);
        gameManager.InGameEvent();
        text.enabled = false;
    }
}
