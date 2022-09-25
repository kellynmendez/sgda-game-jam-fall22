using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float factor = -0.035f;

    PlayerController player = null;
    Vector3 shakeVector = Vector3.zero;
    float shakeDuration = 0f;
    float shakeTimer = 0f;
    float undirectedShakeAmplitude = 0f;
    bool directedShake = true;


    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void LateUpdate()
    {
        // Apply the offset every frame to reposition this object
        Vector3 tempPosition = player.transform.position + (player.velocity * factor);

        if (shakeTimer < shakeDuration)
        {
            if (directedShake)
            {
                tempPosition += shakeVector * Mathf.Sin(Mathf.PI / shakeDuration * shakeTimer);
            }
            else
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                Vector3 randomVector = new Vector3(Mathf.Cos(angle) * undirectedShakeAmplitude, Mathf.Sin(angle) * undirectedShakeAmplitude, 0);
                tempPosition += randomVector * Mathf.Sin(Mathf.PI / shakeDuration * shakeTimer);
            }

            shakeTimer += Time.deltaTime;
        }

        transform.position = tempPosition;
    }
    public void UndirectedShake(float amplitude, float duration)
    {
        if (shakeTimer >= shakeDuration || amplitude >= shakeVector.magnitude)
        {
            directedShake = false;
            shakeDuration = duration;
            undirectedShakeAmplitude = amplitude;
        }
    }

    public void Shake(Vector3 direction, float amplitude, float duration)
    {
        //Debug.Log(shakeTimer+"/"+shakeDuration+" | "+amplitude+"/"+shakeVector.magnitude);
        if (shakeTimer >= shakeDuration || amplitude >= shakeVector.magnitude)
        {
            directedShake = true;
            shakeVector = direction.normalized * amplitude;
            shakeDuration = duration;
            shakeTimer = 0f;
        }
    }

}