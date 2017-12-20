using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 vecTarget;
    public GameObject explosion;
    public GameObject areaOfDamage;
    private float timer = 5f;

    public float speed = 5f;
    public float rotateSpeed = 240f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            vecTarget = PlayerController.spawnPosition;
        }

        if (Input.GetMouseButton(0))
        {
            vecTarget = PlayerController.spawnPosition;
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)vecTarget - rb.position;
        direction.Normalize();

        float rotationAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotationAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
        if (timer <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Instantiate(areaOfDamage, transform.position, transform.rotation);
            Destroy(gameObject);
        }else
        {
            timer -= Time.deltaTime;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(areaOfDamage, transform.position, transform.rotation);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
