using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate;
    [SerializeField] ParticleSystemForceField _field;

    // Player data
    private int _currentHealth = 3;
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

    public void DecreaseHealth(int healthDecr)
    {
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
        DisableDeathObjects();
        _playerIsDead = true;
        _hudMngr.PlayDeathFX();
        StartCoroutine(CameraShake(_duration, _magnitude));
    }

    private void DisableDeathObjects()
    {
        _visualsToDeactivate.SetActive(false);
        _boxCollider.enabled = false;
        Destroy(_rigidBody);
    }

    IEnumerator CameraShake(float duration, float magnitude)
    {
        Debug.Log("shaking");
        Vector2 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector2(x, y);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
