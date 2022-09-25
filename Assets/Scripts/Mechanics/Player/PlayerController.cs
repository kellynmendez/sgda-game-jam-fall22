using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxVelocity = 5f;
    public float acceleration = 1f;

    public Vector3 velocity = Vector3.zero;

    Rigidbody2D body;

    float dragCoefficent = 0.5f;
    float appliedAccelerationX = 0f;
    float appliedAccelerationY = 0f;

    bool isDigging = false;
    PlayerState state = PlayerState.FreeRoam;
    HealthManager _healthMgr;

    public enum PlayerState
    {
        FreeRoam = 0,
        TrashLocked = 1,
        Dead = 2
    }

    // Start is called before the first frame update
    void Start()
    {
        dragCoefficent = acceleration/maxVelocity;
        body = GetComponent<Rigidbody2D>();
        _healthMgr = GetComponent<HealthManager>();
    }

    private void FixedUpdate()
    {
        // If player is not dead, then it can move
        if (state != PlayerState.Dead)
        {
            if (state == PlayerState.FreeRoam)
            {
                dragCoefficent = acceleration / maxVelocity;
                velocity.x = velocity.x + (appliedAccelerationX - velocity.x * dragCoefficent) * Time.fixedDeltaTime;
                velocity.y = velocity.y + (appliedAccelerationY - velocity.y * dragCoefficent) * Time.fixedDeltaTime;
                body.velocity = velocity;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // If player is not dead, then it can move
        if (state != PlayerState.Dead)
        {
            if (_healthMgr.IsPlayerDead())
            {
                state = PlayerState.Dead;
            }
            else
            {
                if (state == PlayerState.FreeRoam) {
                appliedAccelerationX = 0f;
                appliedAccelerationY = 0f;
                if (Input.GetKey(KeyCode.LeftArrow))
                    appliedAccelerationX -= acceleration;
                if (Input.GetKey(KeyCode.RightArrow))
                    appliedAccelerationX += acceleration;
                if (Input.GetKey(KeyCode.DownArrow))
                    appliedAccelerationY -= acceleration;
                if (Input.GetKey(KeyCode.UpArrow))
                    appliedAccelerationY += acceleration;
                }
            }
            
        }
    }

    public void TrashLock(TrashBin trash, float waitTime)
    {
        state = PlayerState.TrashLocked;
        StartCoroutine(MoveToTrash(trash, 0.05f, waitTime));
    }

    public void ApplyVelocity(Vector3 newVelocity){
        velocity += newVelocity;
    }

    public PlayerState GetState()
    {
        return state;
    }

    IEnumerator MoveToTrash(TrashBin trash, float travelTime, float waitTime)
    {
        Vector3 oldPosition = transform.position;
        float elapsed = 0;
        while (elapsed < travelTime && Input.GetKey(KeyCode.X)){
            transform.position = Vector3.Lerp(oldPosition, trash.transform.position, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (Input.GetKey(KeyCode.X)){
            StartCoroutine(DigTrash(trash, waitTime));
        }
        else{
            state = PlayerState.FreeRoam;
            trash.Interupt();
        }
    }

    IEnumerator DigTrash(TrashBin trash, float waitTime)
    {
        isDigging = true;
        trash.EmptyBin();
        while(waitTime > 0 && Input.GetKey(KeyCode.X)){
            waitTime -= Time.deltaTime;
            yield return null;
        }

        isDigging = false;
        state = PlayerState.FreeRoam;

        if (!Input.GetKey(KeyCode.X))
            trash.Interupt();
        
    }
}
