using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float xRadius = 0.5f;
    public float yRadius = 0.5f;

    public float xShakeRate = 1.2f;
    public float yShakeRate = 0.8f;

    float thetaX = 0;
    float thetaY = 90;

    Vector3 minX = Vector3.zero;
    Vector3 maxX = Vector3.zero;
    Vector3 minY = Vector3.zero;
    Vector3 maxY = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        minX = transform.position;
        maxX = transform.position;
        minY = transform.position;
        maxY = transform.position;

        minX.x -= xRadius;
        maxX.x += xRadius;
        minY.y -= yRadius;
        maxY.y += yRadius;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lerpX = Vector3.Lerp(minX, maxX, Mathf.Sin(thetaX*Mathf.Deg2Rad));
        Vector3 lerpY = Vector3.Lerp(minY, maxY, Mathf.Sin(thetaY * Mathf.Deg2Rad));
        thetaX += xShakeRate * Time.deltaTime;
        thetaY += yShakeRate * Time.deltaTime;
        //Debug.Log(lerpX + " " + lerpY);
        transform.position = new Vector3(lerpX.x, lerpY.y, 0);
    }
}
