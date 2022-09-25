using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public GameObject binLight;

    public float respawnDuration = 5;
    public float burnDuration = 5;
    public float emptyingDuration = 5;

    private HUDManager _hudMngr;

    public BinState state = BinState.Full;
    private float timer = 0f;
    private GameObject bLight;

    Light pLight;

    // Start is called before the first frame update
    public enum BinState{
        Full = 0,
        Emptying = 1,
        Burning = 2,
        Empty = 3
    }

    void Start()
    {
        pLight = gameObject.GetComponent<Light>();
        pLight.enabled = false;
        bLight = Instantiate(binLight);
        bLight.transform.parent = transform;
        bLight.SetActive(false);

        _hudMngr = FindObjectOfType<HUDManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != BinState.Full)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                changeState();
        }
    }

    public BinState GetState()
    {
        return state;
    }

    public bool Burn()
    {
        if (state == BinState.Full)
        {
            state = BinState.Burning;

            timer = burnDuration;
            bLight.SetActive(true);
            pLight.enabled = true;
            Debug.Log("Full -> Burning");
            return true;
        }

        return false;
    }

    public bool Interrupt()
    {
        if (state == BinState.Emptying) {

            state = BinState.Empty;
            timer = respawnDuration;
            Debug.Log("Emptying -> Empty via Interuption");
            return true;
         }

        return false;

    }

    public bool EmptyBin()
    {
        if (state == BinState.Full)
        {

            state = BinState.Emptying;
            timer = emptyingDuration;
            Debug.Log("Full -> Emptying");
            return true;
        }

        return false;
    }

    private void changeState() //Change states burning, emptying, and empty to their next available state
    {
        switch (state)
        {
            case BinState.Emptying:
                state = BinState.Empty;
                timer = respawnDuration;
                Debug.Log("Emptying -> Empty");
                break;
            case BinState.Burning:
                state = BinState.Empty;
                timer = respawnDuration;
                bLight.SetActive(false);
                pLight.enabled = false;
                Debug.Log("Burning -> Empty");
                break;
            case BinState.Empty:
                state = BinState.Full;
                Debug.Log("Empty -> Full");
                break;
        }
    }
}
