using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    private static float timeToNextWave = 4f;    //time between waves
    private int enemiesAmt = 5;                  //enemies in one wave
    private float timeBetweenSpawning = 0.5f;    //time between spawning ships
    public enum state { spawning, waiting }     //states for spawn ships

    private state spawnState = state.waiting;

    public Transform leftSpawner;
    public Transform middleSpawner;
    public Transform rightSpawner;
    private MenuManager menuManager;
    private bool middleToLeft = false;                      //for selecting animation if spawned from middle spawner
    private int[] chooseOfSpawner = new int[3] { 1, 2, 3 }; //holding values for choosing spawner
    private int random;                                     //random.range for index of chooseOfSpawner
    private int sideOfSpawn;                                //getting actual values for comparison in if statements

    private float timer;    
    int shipCount = 0;      //number of spawned ships in actual wave for counting them


    void Start ()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
	}

    private void OnEnable()
    {
        GameManager.inGame += TimerSetToZero;
    }
    private void OnDisable()
    {
        GameManager.preparation -= TimerSetToZero;
    }

    private void TimerSetToZero()
    {
        timer = 0f; //first wave appears immediately
        middleToLeft = false;
    }

    void Update ()
    {
        if (menuManager.myState == MenuManager.gameState.InGame)
        {
            if (spawnState == state.spawning)
            {
                if (GameManager.aliveEnemies == 0)
                {
                    spawnState = state.waiting;
                    timer = timeToNextWave;
                    return;
                }
            }

            if (timer <= 0)
            {
                if (spawnState != state.spawning)
                {
                    StartCoroutine(SpawnWing());
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }
        } 
	}

    IEnumerator SpawnWing ()
    {
        spawnState = state.spawning;

        for(int i = 0; i <enemiesAmt; i++)
        {
            random = (int)Random.Range(0f, 3f);
            sideOfSpawn = chooseOfSpawner[random];

            if (sideOfSpawn == 1)
            {
                SpawnShipLeft();
            }else if(sideOfSpawn == 2)
            {
                SpawnShipMiddle();
            }else if(sideOfSpawn == 3)
            {
                SpawnShipRight();
            }
            yield return new WaitForSeconds(timeBetweenSpawning);
        }
        shipCount = 0;
        yield break;
    }

    void SpawnShipLeft()
    {
        GameObject obj = PooledObjectsManager.current.GetPooledSimpleEnemies();
        obj.transform.parent = leftSpawner.transform;
        obj.transform.position = leftSpawner.position;
        obj.transform.rotation = leftSpawner.rotation;
        obj.SetActive(true);
        obj.GetComponent<Animator>().Play("simpleEnemyLeftToRight");
        shipCount++;
    }

    void SpawnShipRight()
    {
        GameObject obj = PooledObjectsManager.current.GetPooledSimpleEnemies();
        obj.transform.parent = rightSpawner.transform;
        obj.transform.position = rightSpawner.position;
        obj.transform.rotation = rightSpawner.rotation;
        obj.SetActive(true);
        obj.GetComponent<Animator>().Play("simpleEnemyRightToLeft");
        shipCount++;
    }

    void SpawnShipMiddle()
    {
        GameObject obj = PooledObjectsManager.current.GetPooledSimpleEnemies();
        obj.transform.parent = middleSpawner.transform;
        obj.transform.position = middleSpawner.position;
        obj.transform.rotation = middleSpawner.rotation;
        obj.SetActive(true);
        if (middleToLeft)
        {
            obj.GetComponent<Animator>().Play("simpleEnemyMiddleToLeft");
        }else
        {
            obj.GetComponent<Animator>().Play("simpleEnemyMiddleToRight");
        }
        middleToLeft = !middleToLeft;
        shipCount++;
    }
}
