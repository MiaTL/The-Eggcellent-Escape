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

    [SerializeField] private LayerMask jumpableGround;

    float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpFloat = 14f;

    //player's different movement states
    private enum MovementState { idle, running, jumping, falling }

    
    [SerializeField] private AudioSource jumpSoundEffect; //Audio Source for Player Jump

    // Start is called before the first frame update
    private void Start()
    {
        //sets values at startup
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        grav = 1;
    }

    // Update is called once per frame
    private void Update()
    {
  
        PlayerMoves();
        PlayerJump();
        SwitchGravity();

        UpdateAnimationState();
    }

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
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpFloat * -1);
            }
            else
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpFloat);
            }

        }
    }

    private void SwitchGravity()
    {
        if (Input.GetKeyDown("z"))
        {
            rb.gravityScale = rb.gravityScale * -1;
            grav *= -1;
        }
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
}


