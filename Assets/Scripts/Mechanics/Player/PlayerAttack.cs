using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public GameObject meleeAttack;
    public float range = 1;
    public float attackDuration = 0.1f;
    public float cooldownDuration = 0.1f;
    bool canAttack = true;

    Vector3 lastValidSpawn = Vector3.right;

    PlayerController playerMov;

    // Start is called before the first frame update
    void Start()
    {
        playerMov = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z) && canAttack && playerMov.GetState() == PlayerController.PlayerState.FreeRoam)
        {
            SoundManager.PlaySound("character_attack_swing");
            canAttack = false;
            StartCoroutine(CooldownTimer(cooldownDuration));
            Vector3 spawnPoint = playerMov.velocity.normalized * range;
            if (spawnPoint == Vector3.zero)
            spawnPoint = lastValidSpawn-transform.position;
            float angle = Mathf.Atan2(spawnPoint.y, spawnPoint.x) * Mathf.Rad2Deg;

            spawnPoint = transform.position + spawnPoint;
            lastValidSpawn = spawnPoint;
            
            GameObject attackInstance = Instantiate(meleeAttack, spawnPoint, Quaternion.Euler(new Vector3(0, 0, angle)));

            if (angle <= 135 && angle >= 45)
            { attackInstance.GetComponent<SpriteRenderer>().flipY = false;
                Vector3 temp = attackInstance.transform.position;
                temp = new Vector3(temp.x, temp.y + 1, temp.z);
                attackInstance.transform.position = temp;
                    }

            if (angle <= -45 && angle >= -135)
            { attackInstance.GetComponent<SpriteRenderer>().flipY = true;
                Vector3 temp = attackInstance.transform.position;
                temp = new Vector3(temp.x, temp.y - 1   , temp.z);
                attackInstance.transform.position = temp;
            }
            if (angle >= 135 && angle <= 180 || angle <= -135 && angle >= -180)
            { 
                attackInstance.GetComponent<SpriteRenderer>().flipY = true;
                Vector3 temp = attackInstance.transform.position;
                temp = new Vector3(temp.x - 1, temp.y, temp.z);
                attackInstance.transform.position = temp;
            }
            if (angle <= 45 && angle >= 0 || angle >= -45 && angle <= 0)
            {
                attackInstance.GetComponent<SpriteRenderer>().flipX = true;
                Vector3 temp = attackInstance.transform.position;
                temp = new Vector3(temp.x + 1, temp.y, temp.z);
                attackInstance.transform.position = temp;
            }

            attackInstance.transform.SetParent(transform);
            Destroy(attackInstance, attackDuration);
        }

    }

    IEnumerator CooldownTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }

}
