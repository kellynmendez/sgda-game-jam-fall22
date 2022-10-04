using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int score = 0;
    public static int _scoreOutput = 0;
    private static int _scaleFactor = 100;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static void IncreaseScore(int n)
    {
        score += n;
        _scoreOutput += (n * _scaleFactor);
        
    }

    public static void ResetScore()
    {
        score = 0;
        _scoreOutput = 0;
    }
}
