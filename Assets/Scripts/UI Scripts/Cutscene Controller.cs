using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    //private Queue<string> sentences
    public TMP_Text text;
    private int counter;
    private int maxCount;

    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject image3;

    [TextArea(3, 10)]
    [SerializeField] public string[] sentences;

    void Start()
    {
        counter = 0;
        maxCount = 9;
        text.text = sentences[counter];
        LeanTween.scaleX(image1, 1, 0);
        LeanTween.scaleX(image2, 0, 0);
        LeanTween.scaleX(image3, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            counter++;
            text.text = sentences[counter];
        }
        if (counter >= 3)
        {
            LeanTween.scaleX(image1, 0, 0);
            LeanTween.scaleX(image2, 1, 0);
            LeanTween.scaleX(image3, 0, 0);
        }
        if (counter >= 6)
        {
            LeanTween.scaleX(image1, 0, 0);
            LeanTween.scaleX(image2, 0, 0);
            LeanTween.scaleX(image3, 1, 0);
        }
        if (counter >= 9)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
