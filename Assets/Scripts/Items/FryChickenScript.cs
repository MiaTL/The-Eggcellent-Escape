using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryChickenScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.currentHealth < player.maxHealth)
            {
                player.currentHealth++;
                Destroy(gameObject);
            }
        }
    }
}
