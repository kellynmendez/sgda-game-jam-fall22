using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _gameScore;

    private void Start()
    {
        if (_gameScore != null)
        {
            AudioManager.Instance.PlayGameScore(_gameScore);
        }
    }
}
