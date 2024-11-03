#region Usings
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class GameManager : MonoBehaviour
{
    #region Variables
    [Header("Level Status")]
    public byte _currentLevel;

    [Header("Player Status")]
    [SerializeField] private GameObject playerReference;
    [SerializeField] private int initialHealth = 3;
    private int health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    [Header("Results UI")]
    [SerializeField] private GameObject _winnerUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Timer _timer;
    #endregion

    #region Initialization
    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        InitializeHealth();
        SubscribeToEnemyEvents();
    }

    private void InitializeHealth()
    {
        health = initialHealth;

        if (playerReference != null)
        {
            UpdateLivesUI(health);
        }
    }
    #endregion

    #region EnemyEvents
    private void SubscribeToEnemyEvents()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromEnemyEvents();
    }

    private void UnsubscribeFromEnemyEvents()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.OnEnemyDeath -= HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        Winner();
    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        UpdateLivesUI(health);

        if (health <= 0)
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
    #endregion

    #region EndGame
    private void Winner()
    {
        if (_timer != null)
        {
            _timer.StopGameTimer();
        }

        Time.timeScale = 0f;
        _winnerUI.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        Destroy(playerReference.gameObject);

        _gameOverUI.SetActive(true);
    }
    #endregion
}
