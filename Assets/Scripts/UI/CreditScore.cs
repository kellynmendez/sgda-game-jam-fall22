using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditScore : MonoBehaviour
{
    [SerializeField] TMP_Text _currentScoreText;
    // Start is called before the first frame update
    void Start()
    {
        _currentScoreText.text = string.Format("{0:000000}", ScoreManager._scoreOutput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
