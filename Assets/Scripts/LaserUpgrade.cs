using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpgrade : MonoBehaviour {

    public float multiplier = 2f;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))

        {
            AddFireSpeed(collider);
        }

        Destroy(gameObject);
    }

    void AddFireSpeed(Collider2D player)
    {
        PlayerController script = player.GetComponent<PlayerController>();
        script.fireRate -= (script.fireRate * Time.deltaTime * multiplier);
    }
}
