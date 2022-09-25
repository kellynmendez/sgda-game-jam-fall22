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

    public float shakeAmp = 0.1f;
    public float shakeDur = 0.1f;

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
        if (Input.GetKey(KeyCode.Z) && canAttack && playerMov.GetState() == PlayerController.PlayerState.FreeRoam)
        {
            canAttack = false;
            StartCoroutine(CooldownTimer(cooldownDuration));
            Vector3 spawnPoint = playerMov.velocity.normalized * range;
            float angle = Vector3.Angle(Vector3.right, spawnPoint);
            cam.Shake(spawnPoint, shakeAmp, shakeDur);
            spawnPoint = transform.position + spawnPoint;

            GameObject attackInstance = Instantiate(meleeAttack, spawnPoint, Quaternion.Euler(new Vector3(0, 0, angle)));
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
