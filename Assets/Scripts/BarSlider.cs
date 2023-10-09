using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class BarSlider : MonoBehaviour
{
    public GameObject bar;

    public int cooldown;
    float nextSwitch;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scaleX(bar, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    public void AnimateBar()
    {
        LeanTween.scaleX(bar, 1, cooldown);
    }

    public void SwitchAnim()
    {
        if (Input.GetKeyDown("z"))
        {
            if (Time.time < nextSwitch)
            {
                return;
            }
            nextSwitch = Time.time + cooldown;
            LeanTween.scaleX(bar, 0, 0);
            AnimateBar();
        }
    }
}
