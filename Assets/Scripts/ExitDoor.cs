using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // Reference to the LevelOverScreen script
    public LevelOverScreen levelOverScreen;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player has reached the door
            levelOverScreen.Setup();
        }
    }
}
