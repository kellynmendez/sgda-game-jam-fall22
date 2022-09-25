using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxVelocity = 5f;
    [SerializeField] float acceleration = 1f;

    public Vector3 velocity = Vector3.zero;

    Rigidbody2D body;
    PlayerState state = PlayerState.FreeRoam;
    HealthManager _healthMgr;

    List<TrashBin> _binList;

    bool _canLoot = true;
    float dragCoefficent = 0.5f;
    float appliedAccelerationX = 0f;
    float appliedAccelerationY = 0f;
    float _lootCooldownTime = 0.1f;
    KeyCode _lightTrash = KeyCode.V;
    KeyCode _lootTrash = KeyCode.X;

    public enum PlayerState
    {
        FreeRoam = 0,
        TrashLocked = 1,
        Dead = 2
    }

    private void Awake()
    {
        dragCoefficent = acceleration/maxVelocity;
        body = GetComponent<Rigidbody2D>();
        _healthMgr = GetComponent<HealthManager>();
        _binList = new List<TrashBin>();
    }

    void Update()
    {
        // If player is not dead, then it can move
        if (state != PlayerState.Dead)
        {
            // Change state to dead if player died
            if (_healthMgr.IsPlayerDead())
            {
                state = PlayerState.Dead;
            }
            else
            {
                if (state == PlayerState.FreeRoam) 
                {
                    // Movement keys
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

                    // Interact keys
                    // If looting
                    if (Input.GetKey(_lootTrash) && _canLoot)
                    {
                        Debug.Log("X was pressed");
                        Debug.Log("bin list count is " + _binList.Count);
                        // There must be a bin that can be interacted with
                        if (_binList.Count != 0)
                        {
                            Debug.Log("bin has something in it");
                            _canLoot = false;
                            TrashBin closestBin = PickTrashBin();
                            if (closestBin != null)
                            {
                                Debug.Log("Closest trash bin is " + closestBin.gameObject.name);
                                // Interacting with the closest trash can
                                TrashLock(closestBin, closestBin.emptyingDuration);
                                Debug.Log("looting trash finished");
                            }
                            else
                            {
                                Debug.Log("no (full) bin exists to interact with");
                            }
                        }
                    }
                    // If lighting
                    if (Input.GetKey(_lightTrash))
                    {

                    }
                }
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TrashBin")
        {
            Debug.Log(collision.gameObject.name + " added to bin list");
            TrashBin bin = collision.gameObject.GetComponent<TrashBin>();
            _binList.Add(bin);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TrashBin")
        {
            Debug.Log(collision.gameObject.name + " removed from bin list");
            TrashBin bin = collision.gameObject.GetComponent<TrashBin>();
            _binList.Remove(bin);
        }
    }

    private TrashBin PickTrashBin()
    {
        // Player's current position
        Vector2 playerPos = transform.position;
        // Track trash can that is closest and its distance from player
        //TrashBin closestBin = null;
        TrashBin closestBin = null;
        float smallestDist = -1;

        foreach (TrashBin check in _binList)
        {
            // if the bin is ready to be interacted with
            if (check.GetState() == TrashBin.BinState.Full)
            {
                // If this is the first full bin to be found
                if (closestBin == null)
                {
                    closestBin = _binList[0];
                    smallestDist = Vector2.Distance(playerPos, closestBin.transform.position);
                }
                // Otherwise see if closer than other bin
                else
                {
                    float distance = Vector2.Distance(playerPos, check.transform.position);
                    // If distance is smaller, then this bin is closer
                    if (distance < smallestDist)
                    {
                        closestBin = check;
                        smallestDist = distance;
                    }
                }
            }
        }
        // return trash bin
        return closestBin;
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void ApplyVelocity(Vector3 newVelocity)
    {
        velocity += newVelocity;
    }

    public void TrashLock(TrashBin trash, float waitTime)
    {
        state = PlayerState.TrashLocked;
        StartCoroutine(MoveToTrash(trash, 0.05f, waitTime));
    }

    IEnumerator MoveToTrash(TrashBin trash, float travelTime, float waitTime)
    {
        Debug.Log("lerping to position");
        // Moving to trash can position
        Vector3 oldPosition = transform.position;
        float elapsed = 0;
        while (elapsed < travelTime && Input.GetKey(_lootTrash)){
            transform.position = Vector3.Lerp(oldPosition, trash.transform.position, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // If player is still holding down the key
        if (Input.GetKey(_lootTrash))
        {
            StartCoroutine(LootTrash(trash, waitTime));
        }
        else
        {
            state = PlayerState.FreeRoam;
        }
    }

    IEnumerator LootTrash(TrashBin trash, float waitTime)
    {
        Debug.Log("Emptying the bin");
        trash.EmptyBin();
        while(waitTime > 0 && Input.GetKey(_lootTrash))
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        if (!Input.GetKey(_lootTrash))
        {
            Debug.Log("interaction ended early, free roam");
            state = PlayerState.FreeRoam;
            trash.Interrupt();
        }

        // Changing state back to roam
        state = PlayerState.FreeRoam;
        Debug.Log("emptying bin is done! starting cooldown");

        StartCoroutine(CooldownTimer(_lootCooldownTime));
    }

    IEnumerator CooldownTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canLoot = true;
        Debug.Log("cool down finished");
    }
}
