using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour {

    public int damage = 5;
    private float timer = 0.1f;
    private GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("MissileTarget") as GameObject;
    }

    public int GetDamage()
    {
        return damage;
    }

    private void Update()
    {
        if(timer <= 0)
        {
            Destroy(gameObject);
            Destroy(target);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
