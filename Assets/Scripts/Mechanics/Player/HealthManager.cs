using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] ParticleSystemForceField _field;
    [SerializeField] int _currentHealth = 250;


    private bool _playerIsDead;

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private HUDManager _hudMngr;
    private float _duration = 0.5f;
    private float _magnitude = 1f;

    private void Awake()
    {
        _playerIsDead = false;
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _hudMngr = FindObjectOfType<HUDManager>();
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void DecreaseHealth(int healthDecr)
    {
        _currentHealth -= healthDecr;
        _hudMngr.SetHealth(_currentHealth);
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
        DisableDeathObjects();
        _playerIsDead = true;
        ScoreManager.ResetScore();
        _hudMngr.PlayDeathFX();
    }

    private void DisableDeathObjects()
    {
        _visualsToDeactivate.SetActive(false);
        _boxCollider.enabled = false;
        Destroy(_rigidBody);
    }
}
