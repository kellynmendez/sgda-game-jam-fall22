using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TMP_Text _currentScoreText;
    [SerializeField] MaskableGraphic _graphic = null;
    [SerializeField] Slider _healthBar;
    [SerializeField] Slider _trashProgressbar;
    bool _exitTrashEarly = false;

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
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }

    public void SetHealth(int health)
    {
        _healthBar.value = health;
    }

    public void StartTrashProgressBar(float duration)
    {
        ActivateTrashBar();
        StartCoroutine(IncrementProgressBar(_trashProgressbar, duration, DeactivateTrashBar));
    }

    public void ExitTrashEarly()
    {
        _exitTrashEarly = true;
    }

    public void ActivateTrashBar()
    {
        _trashProgressbar.gameObject.SetActive(true);
    }
    public void DeactivateTrashBar()
    {
        _trashProgressbar.gameObject.SetActive(false);
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

    public IEnumerator IncrementProgressBar(Slider slider, float duration, System.Action OnComplete = null)
    {
        
        // intial value
        slider.value = 0;

        while (!_exitTrashEarly && slider.value < 1)
        {
            slider.value += Time.deltaTime / duration;
            yield return null;
        }

        if (_exitTrashEarly)
        {
            _exitTrashEarly = false;
            yield break;
        }

        // final value
        slider.value = 1;

        if (OnComplete != null) { OnComplete(); }
        yield break;
    }
}
