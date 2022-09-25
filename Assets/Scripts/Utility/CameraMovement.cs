using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float factor = -0.035f;

    PlayerController player = null;


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void LateUpdate()
    {
        // Apply the offset every frame to reposition this object
        this.transform.position = player.transform.position + (player.velocity * factor);
    }
}