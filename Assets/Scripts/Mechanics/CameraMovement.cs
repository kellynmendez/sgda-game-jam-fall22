using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform _objectToFollow = null;

    Vector3 _objectOffset;

    private void Awake()
    {
        _objectToFollow = FindObjectOfType<PlayerMovement>().transform;
        // Create an offset between this position and the object's position
        _objectOffset = this.transform.position - _objectToFollow.position;
    }

    private void LateUpdate()
    {
        // Apply the offset every frame to reposition this object
        this.transform.position = _objectToFollow.position + _objectOffset;
    }
}