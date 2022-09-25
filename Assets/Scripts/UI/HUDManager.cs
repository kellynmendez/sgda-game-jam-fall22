using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Text _currentScoreText;
    [SerializeField] MaskableGraphic _graphic = null;

    private CameraMovement _camera;

    private float _deathPauseTime = 1f;
    int _currentScore;

    private void Awake()
    {
        _camera = FindObjectOfType<CameraMovement>();
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
        Debug.Log("pausing screen");
        yield return new WaitForSeconds(_deathPauseTime);
        Time.timeScale = 0f;
    }
}
