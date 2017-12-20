using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectsManager : MonoBehaviour {

    public static PooledObjectsManager current;
    public List<GameObject> projectiles;
    public static int projectilesAmt = 10;
    public GameObject projectilePrefab;

    public static int enemyProjectilesAmt = 10;
    public List<GameObject> enemyProjectiles;
    public GameObject enemyProjectilePrefab;

    public List<GameObject> simpleEnemies;
    public static int simpleEnemiesAmt = 10;
    public GameObject simpleEnemyPrefab;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        // instatiating objects on game start and adding them to pooled objects lists

        projectiles = new List<GameObject>();

        for (int i = 0; i < projectilesAmt; i++)
        {
            GameObject obj = Instantiate(projectilePrefab) as GameObject;
            obj.SetActive(false);
            projectiles.Add(obj);
        }

        enemyProjectiles = new List<GameObject>();

        for (int i = 0; i < enemyProjectilesAmt; i++)
        {
            GameObject obj = Instantiate(enemyProjectilePrefab) as GameObject;
            obj.SetActive(false);
            enemyProjectiles.Add(obj);
        }

        simpleEnemies = new List<GameObject>();

        for (int i = 0; i < simpleEnemiesAmt; i++)
        {
            GameObject obj = Instantiate(simpleEnemyPrefab) as GameObject;
            obj.SetActive(false);
            simpleEnemies.Add(obj);
        }
    }

    public GameObject GetPooledEnemyProjectiles()
    {
        // getting objects by calling this function and when find previously spawned, inactive object
        for (int i = 0; i < enemyProjectiles.Count; i++)
        {
            if (!enemyProjectiles[i].activeInHierarchy)
            {
                return enemyProjectiles[i];
            }
        }
        // if runs out of objects, spawns a new one and adds it to the list
        GameObject obj = Instantiate(enemyProjectilePrefab) as GameObject;
        enemyProjectiles.Add(obj);
        return obj;
    }

    public GameObject GetPooledSimpleEnemies()
    {
        for (int i = 0; i < simpleEnemies.Count; i++)
        {
            if (!simpleEnemies[i].activeInHierarchy)
            {
                return simpleEnemies[i];
            }
        }

        GameObject obj = Instantiate(projectilePrefab) as GameObject;
        simpleEnemies.Add(obj);
        return obj;
    }

    public GameObject GetPooledProjectiles()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                return projectiles[i];
            }
        }

        GameObject obj = Instantiate(projectilePrefab) as GameObject;
        projectiles.Add(obj);
        return obj;
    }
}
