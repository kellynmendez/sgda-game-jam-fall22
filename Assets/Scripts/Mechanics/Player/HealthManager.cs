using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] ParticleSystemForceField _field;

    private int _currentHealth = 3;
    private bool _playerIsDead;
    private float _pauseTime = 2f;

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _playerIsDead = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void DecreaseHealth(int healthDecr)
    {
        Debug.Log("Decreasing health: " + _currentHealth);
        _currentHealth -= healthDecr;
        if (_currentHealth <= 0)
        {
            Kill();
        }
    }

    public bool IsPlayerDead()
    {
        return _playerIsDead;
    }

    private void Kill()
    {
        Debug.Log("Player has been killed!");
        DisableDeathObjects();
        _playerIsDead = true;
        //PlayDeathFX();
    }

    private void DisableDeathObjects()
    {
        _visualsToDeactivate.SetActive(false);
        _field.enabled = false;
        _boxCollider.enabled = false;
        Destroy(_rigidBody);
        StartCoroutine(PauseScreen());
    }

    /*
    private void PlayDeathFX()
    {
        Debug.Log("playing death fx");
        if (_deathParticles != null)
        {
            _deathParticles.Play();
        }
        if (_audioSource != null && _deathSFX != null)
        {
            _audioSource.volume = 150;
            _audioSource.PlayOneShot(_deathSFX, _audioSource.volume);
        }
    }
    */

    IEnumerator PauseScreen()
    {
        Debug.Log("pausing screen");
        yield return new WaitForSeconds(_pauseTime);
        Time.timeScale = 0f;
    }
}
