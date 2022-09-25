using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    BinState state = BinState.Full;

    // Start is called before the first frame update
    public enum BinState{
        Full = 0,
        Emptying = 1,
        Burning = 2,
        Empty = 3
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BinState GetState()
    {
        return state;
    }

    public void Burn()
    {

    }

    public void Interrupt()
    {

    }

    internal void EmptyBin()
    {
        
    }
}
