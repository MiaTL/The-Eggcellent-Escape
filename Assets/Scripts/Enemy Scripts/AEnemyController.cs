using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEnemyController : MonoBehaviour
{
    private bool isInvincible;
    private Animator animator;

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;

    public Transform player;

    [SerializeField] GameObject chickenPrefab;

    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 objectPosition = transform.position;

            if (playerPosition.x > objectPosition.x)
            {
                // Player is in front, flip the object to face the player
                FlipOnXAxis(false);
            }
            else
            {
                // Player is behind, flip the object to face away from the player
                FlipOnXAxis(true);
            }
        }
    }

    void FlipOnXAxis(bool flip)
    {
        Vector3 scale = transform.localScale;
        if (flip)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            animator.Play("A_Hit");
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                // deathSoundEffect.Play(); 
                Defeat();
            }
        }
    }

    public void Defeat()
    {
        deathSoundEffect.Play();
        animator.Play("A_Death");
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the "Death" animation has finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // Now, destroy the game object
        Destroy(gameObject);

        int randNum = Random.Range(0, 10);
        if (randNum >= 7)
        {
            GameObject fry = Instantiate(chickenPrefab, transform.position, Quaternion.identity);
            fry.name = chickenPrefab.name;
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        // colliding with player inflicts damage and takes contact damage away from health
    //        PlayerController player = other.gameObject.GetComponent<PlayerController>();
    //        player.HitSide(transform.position.x > player.transform.position.x);
    //        player.TakeDamage(this.contactDamage);
    //    }
    //}
}