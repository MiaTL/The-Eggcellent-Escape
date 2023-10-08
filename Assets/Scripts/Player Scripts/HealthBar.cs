using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController player;
    public GameObject greenBar;
    public GameObject yellowBar;
    public GameObject redBar;
    private int health;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.currentHealth);
        FullHealth();
        MedHealth();
        LowHealth();   
    }

    private void FullHealth()
    {
        if (health == 3)
        {
            LeanTween.scaleX(yellowBar, 0, 0);
            LeanTween.scaleX(redBar, 0, 0);
        }
    }

    private void MedHealth()
    {
        if (health == 2)
        {
            LeanTween.scaleX(greenBar, 0, 0);
            LeanTween.scaleX(redBar, 0, 0);
        }
    }

    private void LowHealth()
    {
        if (health == 1)
        {
            LeanTween.scaleX(greenBar, 0, 0);
            LeanTween.scaleX(yellowBar, 0, 0);
        }
    }
}
