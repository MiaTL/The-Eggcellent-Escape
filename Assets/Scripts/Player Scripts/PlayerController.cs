using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //gets needed info from unity
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    //for gravity function
    int grav;
    public float switchTime;
    float nextSwitch;

    //ability to take damage
    private bool isTakingDamage;
    private bool isInvincible;

    //side hit from
    private bool hitSideRight;

    //health
    private int currentHealth;
    private int maxHealth = 3;

    //determine on what tiles can you jump
    [SerializeField] private LayerMask jumpableGround;

    //speed of character
    float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpFloat = 14f;


    //player's different movement states
    private enum MovementState { idle, running, jumping, falling }

    // Start is called before the first frame update
    private void Start()
    {
        //sets values at startup
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        grav = 1;
        currentHealth = maxHealth;
        nextSwitch = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTakingDamage)
        {
            anim.Play("Player_Hit");
            //Debug.Log("WTF");
            return;
        }
        Debug.Log(Time.time + " : " + nextSwitch);
        Debug.Log(Time.time > nextSwitch);

        PlayerMoves();
        PlayerJump();
        SwitchGravity();

        UpdateAnimationState();
    }

    //PLAYER MOVEMENT FUNCTIONS
    private void PlayerMoves()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);
    }

    private void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if (grav == -1)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpFloat * -1);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpFloat);
            }

        }
    }

    private void SwitchGravity()
    {
        if (Input.GetKeyDown("z"))
        {
            if (Time.time < nextSwitch)
            {
                return;
            }
            nextSwitch = Time.time + switchTime;
            rb.gravityScale = rb.gravityScale * -1;
            grav *= -1;
        }
        //if (Input.GetKeyDown("z"))
        //{
        //    rb.gravityScale = rb.gravityScale * -1;
        //    grav *= -1;
        //}
    }
    //END OF PLAYER MOVEMENT FUNCTIONS

    //PLAYER ANIMATION FUNCTION
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        if (grav == -1)
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }

        //anim.SetInteger("state", (int)state);
    }

    //checks to see if player is grounded
    //terrain is set as a grounded state
    private bool IsGrounded()
    {
        if (grav == -1)
        {
            return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.up, .1f, jumpableGround);
        }
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    //PLAYER DAMAGE FUNCTIONS
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Trap"))
    //    {
    //        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
    //        HitSide(enemy.transform.position.x > transform.position.x);
    //        TakeDamage(enemy.contactDamage);
    //        //Debug.Log("PLAYER HIT");
    //    }
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
    //        HitSide(enemy.transform.position.x > transform.position.x);
    //        TakeDamage(enemy.contactDamage);
    //        //Debug.Log("PLAYER HIT");
    //    }
    //    if (currentHealth <= 0)
    //    {
    //        currentHealth = maxHealth;
    //        RestartLevel();
    //        Die();
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            HitSide(gameObject.transform.position.x > transform.position.x);
            TakeDamage(1);
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
                //Debug.Log("START TAKING DAMAGE");
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
            float hitForceX = 3f;
            float hitForceY = 2f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        anim.Play("Player_Hit", -1, 0f);
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
