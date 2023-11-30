using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private AudioSource playButton;
    public GameObject settingsMenu;
    public float soundDuration = 3.0f;


    public void StartGame()
    {
        playButton.Play();

        StartCoroutine(TransitionWithSoundDelay());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        settingsMenu.SetActive(false);
    }

    public void ToggleSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void Level1()
    {
        playButton.Play();

        StartCoroutine(LevelOneTransition());
    }

    public void Level2()
    {
        playButton.Play();

        StartCoroutine(LevelTwoTransition());
    }

    public void Level3()
    {
        playButton.Play();

        StartCoroutine(LevelThreeTransition());
    }

    private IEnumerator TransitionWithSoundDelay()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LevelOneTransition()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Load the next scene
        SceneManager.LoadScene("Level 1");
    }

    private IEnumerator LevelTwoTransition()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Load the next scene
        SceneManager.LoadScene("Level 2");
    }

    private IEnumerator LevelThreeTransition()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Load the next scene
        SceneManager.LoadScene("Level 3");
    }

}
