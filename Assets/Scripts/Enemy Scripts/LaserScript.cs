using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Transform player;

    float destroyTime;

    public int damage = 1;

    [SerializeField] float laserSpeed = 5f;
    [SerializeField] float destroyDelay;
    [SerializeField] float maxAimDistance = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetLaserSpeed(float speed)
    {
        this.laserSpeed = speed;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        if (Vector2.Distance(transform.position, player.position) <= maxAimDistance)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * laserSpeed;
            sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
            destroyTime = destroyDelay;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            playerController.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
