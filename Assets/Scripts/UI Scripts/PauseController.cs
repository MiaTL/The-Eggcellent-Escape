using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    bool activeMenu;
    public GameObject pauseMenu;
    void Start()
    {
        activeMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            if (activeMenu)
            {
                deactivateMenu();
                activeMenu = false;
            }
            else
            {
                activateMenu();
                activeMenu = true;
            }
        }
    }
    public void activateMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void deactivateMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
