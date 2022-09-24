using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public GameObject meleeAttack;
    public float range = 1;
    public float attackDuration = 0.1f;

    // positive x = right, negative x = left, positive y = up, negative y = down
    Vector3 direction = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow), right = Input.GetKey(KeyCode.RightArrow),
            down = Input.GetKey(KeyCode.DownArrow), up = Input.GetKey(KeyCode.UpArrow);

        if (up && right)
        {
            direction.x = 1;
            direction.y = 1;
        }
        else if (up && left)
        {
            direction.x = -1;
            direction.y = 1;
        }
        else if (down && right)
        {
            direction.x = 1;
            direction.y = -1;
        }
        else if (down && left)
        {
            direction.x = -1;
            direction.y = -1;
        }else if (left)
        {
            direction.x = -1;
            direction.y = 0;
        }
        else if (right)
        {
            direction.x = 1;
            direction.y = 0;
        }
        else if (down)
        {
            direction.y = -1;
            direction.x = 0;
        }
        else if (up)
        {
            direction.y = 1;
            direction.x = 0;
        }



        if (Input.GetKey(KeyCode.Z)){
            float angle = Mathf.Atan2(direction.y, direction.x);
            Vector3 spawnPoint = new Vector3(Mathf.Cos(angle) * range, Mathf.Sin(angle) * range, 0);
            spawnPoint = transform.position + spawnPoint;

            GameObject attackInstance = Instantiate(meleeAttack, spawnPoint, Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg*angle)));
            Destroy(attackInstance, attackDuration);
        }
    }

}
