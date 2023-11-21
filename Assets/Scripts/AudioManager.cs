using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    public Slider masterVolumeSlider;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure there is only one instance of the script
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Find AudioSource using tag
        GameObject audioSourceObject = GameObject.FindWithTag("AudioSource");

        if (audioSourceObject != null)
        {
            // Get the AudioSource component from the found GameObject
            audioSource = audioSourceObject.GetComponent<AudioSource>();

            if (masterVolumeSlider != null && audioSource != null)
            {
                // Load the saved master volume from PlayerPrefs
                float savedMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

                // Initialize the audio source volume based on the loaded master volume
                audioSource.volume = savedMasterVolume;

                // Set the master volume slider value
                masterVolumeSlider.value = savedMasterVolume;
            }
        }
    }

    // Update the volume of the attached audio source based on the master volume slider
    private void UpdateAudioVolume()
    {
        if (masterVolumeSlider != null && audioSource != null)
        {
            // Update the volume of the attached audio source
            audioSource.volume = masterVolumeSlider.value;
        }
    }

    // Called when the master volume slider value changes
    public void OnMasterVolumeChanged()
    {
        // Save the master volume to PlayerPrefs for future sessions
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        UpdateAudioVolume();
    }
}

