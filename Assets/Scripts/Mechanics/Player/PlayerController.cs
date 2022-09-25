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
    Animator animator;

    List<TrashBin> _binList;

    float dragCoefficent = 0.5f;
    float appliedAccelerationX = 0f;
    float appliedAccelerationY = 0f;
    bool isDigging = false;
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
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        // If player is not dead, then it can move
        if (state != PlayerState.Dead)
        {

            if ((velocity.x <= 0.001f && velocity.x >= -0.0001f) && (velocity.y <= 0.001f && velocity.y >= -0.0001f))
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);
            }
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            bool up = (angle <= 135 && angle >= 45);
            bool down = (angle <= -45 && angle >= -135);
            animator.SetBool("UpDown", up || down);
            bool left = (angle >= 135 && angle <= 180 || angle <= -135 && angle >= -180);
            bool right = (angle <= 45 && angle >= 0 || angle >= -45 && angle <= 0);
            if (left == true)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            }

            if (down == true)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;
            }
            animator.SetBool("RightLeft", right || left);
            animator.SetBool("Dig", isDigging);


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
                    if (Input.GetKey(_lootTrash))
                    {
                        // There must be a bin that can be interacted with
                        if (_binList.Count != 0)
                        {
                            TrashBin closestBin = PickTrashBin();
                            if (closestBin != null)
                            {
                                // Changing state to trash lock
                                state = PlayerState.TrashLocked;
                                // Looting the closest trash can
                                StartCoroutine(MoveToLootTrash(closestBin, 0.05f, closestBin.emptyingDuration));
                                // Changing state back to roam
                                state = PlayerState.FreeRoam;
                            }
                        }
                    }
                    // If lighting
                    if (Input.GetKey(_lightTrash))
                    {
                        // There must be a bin that can be interacted with
                        if (_binList.Count != 0)
                        {
                            TrashBin closestBin = PickTrashBin();
                            if (closestBin != null)
                            {
                                // Lighting the closest trash can
                                StartCoroutine(MoveToLightTrash(closestBin, 0.05f));
                            }
                        }
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
            TrashBin bin = collision.gameObject.GetComponent<TrashBin>();
            _binList.Add(bin);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TrashBin")
        {
            TrashBin bin = collision.gameObject.GetComponent<TrashBin>();
            _binList.Remove(bin);
        }
    }

    private TrashBin PickTrashBin()
    {
        // Player's current position
        Vector2 playerPos = transform.position;
        // Track trash can that is closest and its distance from player
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
                    closestBin = check;
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

    IEnumerator MoveToLootTrash(TrashBin trash, float travelTime, float waitTime)
    {
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

    IEnumerator MoveToLightTrash(TrashBin trash, float travelTime)
    {
        trash.Burn();
        // Moving to trash can position
        Vector3 oldPosition = transform.position;
        float elapsed = 0;
        while (elapsed < travelTime)
        {
            transform.position = Vector3.Lerp(oldPosition, trash.transform.position, elapsed / travelTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator LootTrash(TrashBin trash, float waitTime)
    {
        trash.EmptyBin();
        isDigging = true;
        while(waitTime > 0 && Input.GetKey(_lootTrash))
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }

        isDigging = false;

        if (!Input.GetKey(_lootTrash))
        {
            state = PlayerState.FreeRoam;
            trash.Interrupt();
        }
    }
}
