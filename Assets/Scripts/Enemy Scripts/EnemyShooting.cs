using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject laser;
    public Transform laserPos;

    private float timer;
    private GameObject player;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < 55)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                // Trigger the animation
                animator.SetTrigger("RedEyes");

                timer = 0;
                shoot();
            }
        }

    }

    void shoot()
    {
        Instantiate(laser, laserPos.position, Quaternion.identity);
    }

}
