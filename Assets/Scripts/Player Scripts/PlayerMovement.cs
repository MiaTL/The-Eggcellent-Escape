using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //gets needed info from unity
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    //for gravity function
    int grav;

    //damage variables
    private bool isTakingDamage;
    private bool isInvincible;

    //check to see if hti from the right side
    private bool hitSideRight;

    //health variables
    private int currentHealth;
    private int maxHealth = 3; 

    [SerializeField] private LayerMask jumpableGround;

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
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);

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

        if (Input.GetKeyDown("z"))
        {
            rb.gravityScale = rb.gravityScale * -1;
            grav *= -1;
        }

        UpdateAnimationState();
    }

    //based off key presses, will move player horizontally and vertically. Will flip player via y axis when going left and right
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
            float hitForceX = 0.5f;
            float hitForceY = 1.5f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;

    }

    //die function x_x
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }
}


