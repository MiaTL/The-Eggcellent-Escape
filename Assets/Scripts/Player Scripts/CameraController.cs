using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    public bool bounds;
    public Vector3 maxCamPos;
    public Vector3 minCamPos;

    private Vector2 velocity; 

    public float smoothTimeY; 
    public float smoothTimeX;

    private void Update()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);

        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCamPos.x, maxCamPos.x),
            Mathf.Clamp(transform.position.y, minCamPos.y, maxCamPos.y),
            Mathf.Clamp(transform.position.z, minCamPos.z, maxCamPos.z));
        }

    }
}
