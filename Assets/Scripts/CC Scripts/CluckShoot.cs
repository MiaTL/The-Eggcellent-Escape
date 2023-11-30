using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluckShoot : MonoBehaviour
{
    public GameObject laser;
    public Transform laserPos;
    public GameObject chickPre;


    public GameObject healthBar;
    public GameObject healthOutline;
    public GameObject ccText;

    private float timer;
    private float chickTimer;
    private bool chickBool;
    private bool laserBool;
    private GameObject player;
    private Animator animator;

    //Laser sound effect
    [SerializeField] private AudioSource laserSoundEffect;
    [SerializeField] private AudioSource chickSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        chickBool = false;
        laserBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 80)
        {
            healthBar.SetActive(true);
            healthOutline.SetActive(true);
            ccText.SetActive(true);
            timer += Time.deltaTime;
            chickTimer += Time.deltaTime;

            if (chickTimer > 2f && !laserBool)
            {
                chickBool = true;
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!stateInfo.IsName("CC_Death"))
                {
                    animator.Play("CC_Punch");

                    chickTimer = 0;

                    if (stateInfo.IsName("CC_Punch"))
                    {

                        shootChicken();
                        chickSoundEffect.Play();
                    }
                }
            }

            if (timer > 1.5f && !chickBool)
            {
                laserBool = true;
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (!stateInfo.IsName("CC_Death"))
                {
                    animator.Play("CC_RedEyes");

                    timer = UnityEngine.Random.Range(0,2);

                    if (stateInfo.IsName("CC_RedEyes"))
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
        animator.Play("CC_Walk");
        laserBool = false;
    }

    void shootChicken()
    {
        Instantiate(chickPre, laserPos.position, Quaternion.identity);
        animator.Play("CC_Walk");
        chickBool = false;
    }
}
