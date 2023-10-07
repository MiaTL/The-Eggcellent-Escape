using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    //ability to take damage
    private bool isTakingDamage;
    private bool isInvincible;

    //side hit from
    private bool hitSideRight;

    //health
    private int currentHealth;
    private int maxHealth = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    //if player collides with something tagged as "Trap" then player will die: Future plans - Health will be lost till 0 and trigger death
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            HitSide(enemy.transform.position.x > transform.position.x);
            TakeDamage(enemy.contactDamage);
            //Debug.Log("PLAYER HIT");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            HitSide(enemy.transform.position.x > transform.position.x);
            TakeDamage(enemy.contactDamage);
            //Debug.Log("PLAYER HIT");
        }
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            RestartLevel();
            Die();
        }
    }

    //determine which side player was hit from
    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    //set player's invincibility status
    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    //function to handle damage
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                Debug.Log("START TAKING DAMAGE");
                StartDamageAnimation();
            }
        }
    }

    //damage taking function animator
    void StartDamageAnimation()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            isInvincible = true;
            float hitForceX = 100f;
            float hitForceY = 3f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        //anim.Play("Player_Hit", -1, 0f);
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    //Restarts level, future plans: Make it go to game over screen
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
