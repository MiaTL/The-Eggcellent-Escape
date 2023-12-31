using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

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
    private bool hitSideRight; //side hit from

    //health
    private int currentHealth;
    public int maxHealth = 3;
    private int currentShield;
    public int maxShield = 1;
    public GameObject greenBar;
    public GameObject yellowBar;
    public GameObject redBar;
    public GameObject shieldBar;

    //Gun variables
    private bool isShooting;
    private float nextBullet;
    [SerializeField] float fireRate = 2f;
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 35f;
    [SerializeField] Transform bulletShootPosition;
    [SerializeField] GameObject bulletPrefab;

    //determine on what tiles can you jump
    [SerializeField] private LayerMask jumpableGround; 

    //speed of character
    float dirX = 0f;
    bool isFacingRight;
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float jumpFloat = 45f;


    //player's different movement states
    private enum MovementState { idle, running, jumping, falling, damage }

    //Player Sound Effects
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource shootSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;
    [SerializeField] private AudioSource itemSoundEffect;
    [SerializeField] private AudioSource shieldSoundEffect;
    //UI Screens
    public GameOverScreen gameOverScreen;

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
        currentShield = 0;
        LeanTween.scaleX(shieldBar, 0, 0);
        nextSwitch = 0;
        isFacingRight = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isTakingDamage)
        {
            anim.Play("Player_Hit");
            return;
        }

        PlayerMoves();
        PlayerJump();
        //PlayerShootInput();
        //SwitchGravity();

        UpdateAnimationState();
    }

    //Controller Testing Functions (WORKING)
    public void Move(InputAction.CallbackContext context)
    {
        dirX = context.ReadValue<Vector2>().x;
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);
    }
    private void PlayerMoves()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * dirX, rb.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
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


    //PLAYER MOVEMENT FUNCTIONS


    
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
    
    public void SwitchGravity(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time < nextSwitch)
            {
                return;
            }
            nextSwitch = Time.time + switchTime;
            rb.gravityScale = rb.gravityScale * -1;
            grav *= -1;
        }
    }
    //END OF PLAYER MOVEMENT FUNCTIONS

    //PLAYER ANIMATION FUNCTION
    private void UpdateAnimationState()
    {
        Vector3 p = bulletShootPosition.localPosition;
        MovementState state;
        if (isTakingDamage)
        {
            state = MovementState.damage;
        }
        if (dirX > 0f)
        {
            isFacingRight = true;
            p.x *= Mathf.Abs(p.x);
            bulletShootPosition.localPosition = p;
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            isFacingRight = false;
            p.x = Mathf.Abs(p.x) * - 1;
            bulletShootPosition.localPosition = p;
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

        anim.SetInteger("state", (int)state);
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
        if (collision.gameObject.CompareTag("Oil"))
        {
            HitSide(gameObject.transform.position.x > transform.position.x);
            TakeDamage(3);
            //Debug.Log("PLAYER HIT");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            HitSide(enemy.transform.position.x > transform.position.x);
            TakeDamage(enemy.contactDamage);
            //Debug.Log("PLAYER HIT");
        }
        if (collision.gameObject.CompareTag("Enemy2"))
        {
            AEnemyController enemy = collision.gameObject.GetComponent<AEnemyController>();
            HitSide(enemy.transform.position.x > transform.position.x);
            TakeDamage(enemy.contactDamage);
            //Debug.Log("PLAYER HIT");
        }
        if (collision.gameObject.CompareTag("Health"))
        {
            if (currentHealth < maxHealth)
            {
                itemSoundEffect.Play();
                getHealth(1);
                Destroy(collision.gameObject);
                HealthBar();
            }
        }
        if (collision.gameObject.CompareTag("Shield") && currentShield < maxShield)
        {
            shieldSoundEffect.Play();
            currentShield++;
            Destroy(collision.gameObject);
            HealthBar();
        }
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            //RestartLevel();
            Die();
            //gameOverScreen.Setup();
            //Die();
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
            if (currentShield == 1)
            {
                currentShield--;
                HealthBar();
                damage--;
                if (damage == currentHealth)
                {
                    Die();
                }
                else
                {
                    StartDamageAnimation();
                }
            }
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                deathSoundEffect.Play();
                Die();
            }
            else
            {
                //Debug.Log("START TAKING DAMAGE");
                HealthBar();
                StartDamageAnimation();
            }
        }
    }

    public void getHealth(int heatlh)
    {
        currentHealth += heatlh;
        HealthBar();
    }

    //HEALTH BAR FUNCTION
    private void HealthBar()
    {
        if (currentShield == 1)
        {
            LeanTween.scaleX(shieldBar, 1, 0);
        }
        if (currentShield == 0)
        {
            LeanTween.scaleX(shieldBar, 0, 0);
        }
        if (currentHealth == 3)
        {
            LeanTween.scaleX(greenBar, 1, 0);
            LeanTween.scaleX(yellowBar, 0, 0);
            LeanTween.scaleX(redBar, 0, 0);
        }
        if (currentHealth == 2)
        {
            LeanTween.scaleX(greenBar, 0, 0);
            LeanTween.scaleX(yellowBar, 1, 0);
            LeanTween.scaleX(redBar, 0, 0);
        }
        if (currentHealth == 1)
        {
            LeanTween.scaleX(greenBar, 0, 0);
            LeanTween.scaleX(yellowBar, 0, 0);
            LeanTween.scaleX(redBar, 1, 0);
        }
        if (currentHealth == 0)
        {
            LeanTween.scaleX(greenBar, 0, 0);
            LeanTween.scaleX(yellowBar, 0, 0);
            LeanTween.scaleX(redBar, 0, 0);
        }
    }

    //damage taking function animator
    void StartDamageAnimation()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            isInvincible = true;
            float hitForceX = 15f;
            float hitForceY = 10f;
            if (hitSideRight) hitForceX = -hitForceX;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);
        }
    }

    void StopDamageAnimation()
    {
        isTakingDamage = false;
        isInvincible = false;
        Debug.Log("Reseting anim");
        anim.Play("Player_Hit", -1, 0f);
        anim.Play("Player_Idle");
    }

    //Shooting functions
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (Time.time < nextBullet)
            {
                return;
            }
            nextBullet = Time.time + fireRate;
            shootSoundEffect.Play();
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPosition.position, Quaternion.identity);
        bullet.name = bulletPrefab.name;

        //set damage, speed, direction
        bullet.GetComponent<BulletScript>().SetDamageValue(bulletDamage);
        bullet.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<BulletScript>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        //shootSoundEffect.Play(); // double effect not needed
        bullet.GetComponent<BulletScript>().Shoot();
    }

    private void Die()
    {
        
        currentHealth = 0;
        HealthBar();
        //gameOverScreen.Setup();
        anim.Play("Player_Death");
        rb.bodyType = RigidbodyType2D.Static;
        //anim.Play("Player_Death");
    }

    void gameOverSetup()
    {
        gameOverScreen.Setup();
    }

    //Restarts level, future plans: Make it go to game over screen
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
