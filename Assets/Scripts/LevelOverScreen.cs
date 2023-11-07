using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverScreen : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void NextLevelButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
}
