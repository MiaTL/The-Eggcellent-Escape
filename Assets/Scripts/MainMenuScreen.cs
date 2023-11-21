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

    private IEnumerator TransitionWithSoundDelay()
    {
        // Wait for the sound to finish playing
        yield return new WaitForSeconds(soundDuration);

        // Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
