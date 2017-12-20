using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float movementSpeed;
    private float padding = 0.25f;      //padding to get whole ship inside camera view, not just a half of it if close to edge
    private float projectileSpeed = 20f;
    private float cooldown = 10f;    //cooldown of missile
    
    public float playerHealth = 3f; //health
    public AudioClip audio;         //laser shoot 
    public GameObject explosion;    //prefab of an explosion of ship
    public Transform leftGun;       //left and right gun objects on which projectiles are activating
    public Transform rightGun;      
    public float fireRate = 1f;     //smaller means higher firerate
    private GameManager gameManager; 
    public GameObject Missile;      //missile prefab
    public Lives lives;             //lives script to showing actual hp
    public GameObject target;       //target of missile prefab
    public static Vector3 spawnPosition; //position for spawning missile
    public GameObject missileReady;     //object "missile ready", activating by ship

    private bool isShooting;
    private float missileCooldown;
    private float tiltSpeed = 0.7f;         // modifier, sensitivity of tilt movement
    private Vector3 movement = new Vector3();
    private MenuManager menuManager;
    private Animator anim;          //animation of ship entering battlezone
    private GameObject sprite;      //sprite of player ship
    private GameObject thruster;    //thruster of player ship
    private PolygonCollider2D coll;
    

    float xmin;
    float xmax;


    private void Start()
    {
        menuManager = GameObject.FindObjectOfType<MenuManager>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        coll = GetComponent<PolygonCollider2D>();

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
        anim.enabled = false;
        transform.Translate(0, -6f, 0);
        sprite = GameObject.Find("Sprite");
        thruster = GameObject.Find("Thruster");
        coll.enabled = false;
        missileReady.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.preparation += PreparingToBattle;
        GameManager.inGame += PlayingBehaviour;
    }
    private void OnDisable()
    {
        GameManager.preparation -= PreparingToBattle;
        GameManager.inGame -= PlayingBehaviour;
    }

    //When touching UI, prevents spawning missile on button "Pause" touch
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //Things to activate that can be used during countdown time
    void PreparingToBattle()
    {
        anim.enabled = true;
        playerHealth = 3;
        sprite.SetActive(true);
        thruster.SetActive(true);
        anim.Play("PlayerGetReady");
        missileCooldown = cooldown;
        coll.enabled = true;
    }

    //Getting ready to the actual game
    void PlayingBehaviour()
    {
        anim.enabled = false;
        if (!isShooting)
        {
            isShooting = true;
            InvokeRepeating("Fire", 0.000002f, fireRate);
        }
        lives.LivesUpdate(playerHealth);
    }


    //fire get projectiles from list in PooledObjectsManager
    void Fire()
    {
        GameObject obj =  PooledObjectsManager.current.GetPooledProjectiles();
        obj.transform.position = leftGun.position;
        obj.transform.rotation = leftGun.rotation;
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        GameObject obj1 = PooledObjectsManager.current.GetPooledProjectiles();
        obj1.transform.position = rightGun.position;
        obj1.transform.rotation = rightGun.rotation;
        obj1.SetActive(true);
        obj1.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);

        AudioSource.PlayClipAtPoint(audio, transform.position);
    }


    //on hit by enemy projectiles, getting damage and calling Death() when HP drops below 0
    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyProjectile enemyProjectile = collision.gameObject.GetComponent<EnemyProjectile>();
        if (enemyProjectile)
        {
            playerHealth -= enemyProjectile.PlayerGetDamage();
            lives.LivesUpdate(playerHealth);
            enemyProjectile.PlayerHit();
            if (playerHealth <= 0)
            {
                Death();
            }
        }
    }


    //Deactivating some objects on death so the player ship will be not shown till the next game
    void Death()
    {
        coll.enabled = false;
        Instantiate(explosion, transform.position, transform.rotation);
        sprite.SetActive(false);
        thruster.SetActive(false);
        CancelInvoke("Fire");
        isShooting = false;
        gameManager.Invoke("LoseEvent", 0.1f);
    }

    void Update () {

        //movement only ingame
        if(menuManager.myState == MenuManager.gameState.InGame)
        {
            //mobile movement on x axis
            movement.x = Input.acceleration.x * tiltSpeed;
            transform.Translate(movement.x, 0, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                InvokeRepeating("Fire", 0.000001f, fireRate);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                CancelInvoke("Fire");
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }

            //cooldown timer
            missileCooldown -= Time.deltaTime;
            //Checking if missile is ready, displaying it and spawning missile 
            if (missileCooldown <= 0)
            {
                missileReady.SetActive(true);

                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    SpawnMissile();
                }
                else if (Input.touchCount > 0 && !IsPointerOverUIObject())
                {
                    SpawnMissile();
                }
            }
        }






        //Clamping player in game window
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }


    //Actual Spawning method that spawn a target icon also
    void SpawnMissile()
    {
        spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0.0f;
        Instantiate(target, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        Instantiate(Missile, transform.position, transform.rotation);
        missileCooldown = cooldown;
        missileReady.SetActive(false);
    }
}