using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxVelocity = 5f;
    public float acceleration = 1f;

    public Vector3 velocity = Vector3.zero;

    Rigidbody2D body;

    float dragCoefficent = 0.5f;
    float appliedAccelerationX = 0f;
    float appliedAccelerationY = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        dragCoefficent = acceleration/maxVelocity;
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        dragCoefficent = acceleration/maxVelocity;
        velocity.x = velocity.x + (appliedAccelerationX - velocity.x*dragCoefficent)*Time.fixedDeltaTime;
        velocity.y = velocity.y + (appliedAccelerationY - velocity.y*dragCoefficent)*Time.fixedDeltaTime;
        body.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        appliedAccelerationX = 0f;
        appliedAccelerationY = 0f;
        if(Input.GetKey(KeyCode.LeftArrow))
            appliedAccelerationX -= acceleration;
        if(Input.GetKey(KeyCode.RightArrow))
            appliedAccelerationX += acceleration;
        if(Input.GetKey(KeyCode.DownArrow))
            appliedAccelerationY -= acceleration;
        if(Input.GetKey(KeyCode.UpArrow))
            appliedAccelerationY += acceleration;
    }

    public void applyVelocity(Vector3 newVelocity){
        velocity += newVelocity;
    }
}
