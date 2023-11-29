using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScript : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool pauseOn;

    private void Start()
    {
        pauseOn = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseOn) // not working I have no idea why i think because it sets timescale to 0
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
                pauseOn = false;
            }
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            pauseOn = true;
        }
    }
}
