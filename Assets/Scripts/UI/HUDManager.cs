using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Text _currentScoreText;
    [SerializeField] MaskableGraphic _graphic = null;

    private float _deathPauseTime = 1f;
    private int _score = ScoreManager._scoreOutput;

    private void Update()
    {
        if(_score != ScoreManager._scoreOutput)
        {
            _score = ScoreManager._scoreOutput;
            UpdateScoreText(_score);
        }
    }

    public void UpdateScoreText(int score)
    {
        _currentScoreText.text = score.ToString();
    }

    public void PlayDeathFX()
    {
        // Change screen color
        _graphic.color = new Color(1, 0, 0, .2f);
        // Start pause
        StartCoroutine(PauseScreen());
    }

    IEnumerator PauseScreen()
    {
        yield return new WaitForSeconds(_deathPauseTime);
        Time.timeScale = 0f;
    }
}
