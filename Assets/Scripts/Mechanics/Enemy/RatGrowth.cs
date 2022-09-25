using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatGrowth : MonoBehaviour
{
    ParticleSystem system;
    public float intercept = 0;
    public float logBase = 5f;
    int lastScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        system = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ScoreManager.score != lastScore)
        {
            ParticleSystem.EmissionModule emissions = system.emission;
            emissions.rateOverTime = calculate();
            lastScore = ScoreManager._scoreOutput;
        }
    }

    float calculate()
    {
        return Mathf.Log(ScoreManager._scoreOutput / intercept, logBase);
    }
}
