using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private bool isInvincible;

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;

    public Transform player;

    [SerializeField] GameObject chickenPrefab;
  

    //Sound Effects
    [SerializeField] private AudioSource deathSoundEffect;
    public float soundDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
            Mathf.Clamp(currentHealth, 0, maxHealth); 
            if (currentHealth <= 0)
            {
                deathSoundEffect.Play(); // not working
                StartCoroutine(DefeatWithSoundDelay());
            }
        }
    }

    public void Defeat()
    {
        
        Destroy(gameObject);
        int randNum = Random.Range(0, 10);
        if (randNum >= 7)
        {
            GameObject fry = Instantiate(chickenPrefab, transform.position, Quaternion.identity);
            fry.name = chickenPrefab.name;
        }
    }

    private IEnumerator DefeatWithSoundDelay()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Defeat enemy
        Defeat();
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
