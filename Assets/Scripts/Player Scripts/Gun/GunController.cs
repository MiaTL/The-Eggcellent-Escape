using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sprite;
    int grav;
    public float switchTime = 1f;
    float nextSwitch;
    float dirX = 0f;
    float originalY;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        grav = 1;
        nextSwitch = 0;
        originalY = sprite.transform.localPosition.y;
}

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        GravityFlip();
        UpDateAnim();
    }

    private void GravityFlip()
    {
        if (Input.GetKeyDown("z"))
        {
            if (Time.time < nextSwitch)
            {
                return;
            }
            nextSwitch = Time.time + switchTime;
            grav *= -1;
        }
    }

    private void UpDateAnim()
    {
        if (dirX > 0f)
        {
            sprite.flipX = false;
            sprite.transform.LeanSetLocalPosX(Mathf.Abs(sprite.transform.localPosition.x));
        }
        else if (dirX < 0f)
        {
            sprite.flipX = true;
            sprite.transform.LeanSetLocalPosX(Mathf.Abs(sprite.transform.localPosition.x) * -1);
        }
        if (grav == -1)
        {
            sprite.flipY = true;
            sprite.transform.LeanSetLocalPosY(originalY * -1);
        }
        else
        {
            sprite .flipY = false;
            sprite.transform.LeanSetLocalPosY(originalY);
        }
    }
}
