using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TMP_Text _currentScoreText;
    [SerializeField] MaskableGraphic _graphic = null;
    [SerializeField] Slider slider;

    HealthManager _healthMngr;
    int _health;

    private float _deathPauseTime = 1f;
    private int _score = ScoreManager._scoreOutput;

    private void Awake()
    {
        _healthMngr = FindObjectOfType<HealthManager>();
        _health = _healthMngr.GetCurrentHealth();
        SetMaxHealth();

        UpdateScoreText(0);
    }

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
        _currentScoreText.text = string.Format("{0:000000}", score);
    }

    private void SetMaxHealth()
    {
        slider.maxValue = _health;
        slider.value = _health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
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
