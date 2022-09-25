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

    [SerializeField] Sprite _barrel1;
    [SerializeField] Sprite _barrel2;
    [SerializeField] Sprite _barrel3;
    [SerializeField] Sprite _barrel4;

    SpriteRenderer _TEMP;

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
        bLight = Instantiate(binLight, this.transform.position, Quaternion.Euler(90f,0f,0f));
        bLight.transform.parent = transform;
        bLight.SetActive(false);
        _TEMP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

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

            _TEMP.sprite = _barrel4;

            SoundManager.PlaySound("sfx_barrel_lit2");
            SoundManager.PlaySound("sfx_barrel_fireambience");
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
            _hudMngr.ExitTrashEarly();
            state = BinState.Empty;
            _TEMP.sprite = _barrel2;
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
            _hudMngr.StartTrashProgressBar(emptyingDuration);
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
                _TEMP.sprite = _barrel2;
                timer = respawnDuration;
                Debug.Log("Emptying -> Empty");
                break;
            case BinState.Burning:
                state = BinState.Empty;
                _TEMP.sprite = _barrel3;
                timer = respawnDuration;
                bLight.SetActive(false);
                pLight.enabled = false;
                Debug.Log("Burning -> Empty");
                break;
            case BinState.Empty:
                state = BinState.Full;
                _TEMP.sprite = _barrel1;
                Debug.Log("Empty -> Full");
                break;
        }
    }
}
