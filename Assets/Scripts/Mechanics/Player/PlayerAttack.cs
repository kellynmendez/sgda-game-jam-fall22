using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    CameraMovement cam;
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
        cam = FindObjectOfType<CameraMovement>();
        playerMov = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canAttack && playerMov.GetState() == PlayerController.PlayerState.FreeRoam)
        {
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
            { attackInstance.GetComponent<SpriteRenderer>().flipY = false; }
            if (angle <= -45 && angle >= -135)
            { attackInstance.GetComponent<SpriteRenderer>().flipY = true; }
            if (angle >= 135 && angle <= 180 || angle <= -135 && angle >= -180)
            { attackInstance.GetComponent<SpriteRenderer>().flipY = true; }
            if (angle <= 45 && angle >= 0 || angle >= -45 && angle <= 0)
            { attackInstance.GetComponent<SpriteRenderer>().flipX = true; }

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
