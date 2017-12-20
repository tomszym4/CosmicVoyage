using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public int health;
    public float enemyProjectileSpeed = 10f;
    public float shotsPerSecond = 0.5f;     //smaller means more per second
    public int scoreValue = 150;            //score for destroying enemy
    public AudioClip laserSound;
    public GameObject enemyProjectilePrefab;
    public GameObject explosion;
    public GameObject laserUpgrade;     //prefab of upgrade for player ship

    private ScoreKeeper score;

    private void Start()
    {
        score = GameObject.FindObjectOfType<ScoreKeeper>();
    }

    private void OnEnable()
    {
        health = 2;
        GameManager.aliveEnemies++;
    }
    private void OnDisable()
    {
        GameManager.aliveEnemies--;
    }


    private void Update()
    {
        //firing based on probability
        float probability = Time.deltaTime * shotsPerSecond;
        if (Random.value < probability)
        {
            EnemyFire();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if(projectile)
        {
            health -= projectile.GetDamage();
            Debug.Log("Enemy Hit");
        }
        MissileExplosion missile = collision.gameObject.GetComponent<MissileExplosion>();
        if(missile)
        {
            health -= missile.GetDamage();
        }
        if (health <= 0)
        {
            Death();
        }

        DeathZone deathzone = collision.gameObject.GetComponent<DeathZone>();
        if (deathzone)
        {
            gameObject.SetActive(false);
        }

    }

    void Death()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        score.Score(scoreValue);
        gameObject.SetActive(false);
        GameManager.killedEnemies++;
        //probability of droping laser upgrade for player
        float upgradeProbability = Random.Range(0f, 10.0f);
        if (upgradeProbability > 9f)
        {
            GameObject obj = Instantiate(laserUpgrade, transform.position, transform.rotation) as GameObject;
            obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -5, 0);
        }
    }

    void EnemyFire()
    {
        GameObject obj = PooledObjectsManager.current.GetPooledEnemyProjectiles();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -enemyProjectileSpeed, 0);

        AudioSource.PlayClipAtPoint(laserSound, transform.position);
    }

    //void AnimDecider()
    //{
    //    //playing animation based on spawn position
    //    Animator simpleEnemyAnim = GetComponentInChildren<Animator>();
    //    if (transform.position.x < 0)
    //    {
    //        simpleEnemyAnim.Play("simpleEnemyLeftToRight");
    //    }
    //    if (transform.position.x == 0)
    //    {
    //        bool midToRight = true;
    //        if (midToRight)
    //        {
    //            simpleEnemyAnim.Play("simpleEnemyMiddleToRight");
    //            midToRight = false;
    //        }
    //        else
    //        {
    //            simpleEnemyAnim.Play("simpleEnemyMiddleToLeft");
    //            midToRight = true;
    //        }
    //    }
    //    if (transform.position.x > 0)
    //    {
    //        simpleEnemyAnim.Play("simpleEnemyRightToLeft");
    //    }
    //}
}
