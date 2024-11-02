using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private GameObject _playerReference;
    [SerializeField] private int _initialHealth = 3;
    private int _health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    [Header("Results UI")]
    [SerializeField] private GameObject _winner;
    [SerializeField] private GameObject _gameOver;


    void Start()
    {
        _health = _initialHealth;

        if (_playerReference != null)
        {
            UpdateLivesUI(_health);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        UpdateLivesUI(_health);

        if (_health <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI(int currentHealth)
    {
        for (int i = 0; i < _lifeImages.Count; i++)
        {
            _lifeImages[i].enabled = i < currentHealth;
        }
    }

    private void Winner()
    {
        _winner.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        Destroy(_playerReference);

        _gameOver.SetActive(true);
    }
}
