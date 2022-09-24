using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Text _currentScoreText;

    int _currentScore;

    private void Update()
    {
        // TODO how player gets points
        // if player picks up trash, call IncreaseScore
    }

    public void IncreaseScore(int scoreIncr)
    {
        _currentScore += scoreIncr;
        _currentScoreText.text = _currentScore.ToString();
    }
}
