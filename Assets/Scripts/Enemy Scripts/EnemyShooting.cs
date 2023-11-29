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

    //Laser sound effect
    [SerializeField] private AudioSource laserSoundEffect; 

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
        //Debug.Log(distance);

        if (distance < 55)
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!stateInfo.IsName("Death"))
                {
                    animator.Play("RedEyes");

                    timer = 0;

                    if (stateInfo.IsName("RedEyes"))
                    {
                        shoot();
                        laserSoundEffect.Play();
                    }
                }
            }
        }

    }

    void shoot()
    {
        Instantiate(laser, laserPos.position, Quaternion.identity);
        animator.Play("BaseChickenWalk");
    }

}

