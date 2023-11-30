using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOutline : MonoBehaviour
{
    public Outline outlineEffect;

    void Start()
    {
        // Get the Outline component from the button's GameObject
        outlineEffect = GetComponent<Outline>();

        // Ensure the Outline component exists and is initially disabled
        if (outlineEffect != null)
        {
            outlineEffect.enabled = false;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Enable the outline effect when the button is selected
        if (outlineEffect != null)
        {
            outlineEffect.enabled = true;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Disable the outline effect when the button is deselected
        if (outlineEffect != null)
        {
            outlineEffect.enabled = false;
        }
    }



}

