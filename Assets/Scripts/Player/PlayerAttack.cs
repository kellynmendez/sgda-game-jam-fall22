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
        if (Input.GetKey(KeyCode.LeftArrow) && direction.x > -1)
            direction = new Vector3(-1, direction.y, 0);
        if (Input.GetKey(KeyCode.RightArrow) && direction.x > 1)
            direction = new Vector3(1, direction.y, 0);
        if (Input.GetKey(KeyCode.DownArrow))
            direction = new Vector3(direction.y, -1, 0);
        if (Input.GetKey(KeyCode.UpArrow))
            direction = new Vector3(direction.y, 1, 0);

        if (Input.GetKey(KeyCode.Z)){
            float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            Debug.Log(angle);
            Vector3 spawnPoint = new Vector3(Mathf.Cos(angle) * range, Mathf.Sin(angle) * range, 0);
            spawnPoint = transform.position + spawnPoint;
            GameObject attackInstance = Instantiate(meleeAttack, spawnPoint, Quaternion.Euler(new Vector3(0, 0, angle)));
            Destroy(attackInstance, attackDuration);
        }
    }

}
