using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isInvincible;

    public int currentHealth;
    public int maxHealth = 1;
    public int contactDamage = 1;

    [SerializeField] GameObject chickenPrefab;

    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
                //deathSoundEffect.Play();
                Defeat();
            }
        }
    }

    public void Defeat()
    {
        //deathSoundEffect.Play(); // not playing on death I do not know why...
        Destroy(gameObject);
        GameObject fry = Instantiate(chickenPrefab, transform.position, Quaternion.identity);
        fry.name = chickenPrefab.name;
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
