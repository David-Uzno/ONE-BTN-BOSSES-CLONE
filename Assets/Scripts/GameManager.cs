using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Status")]
    public byte _currentLevel;

    [Header("Results UI")]
    [SerializeField] private GameObject _winnerUI;
    [SerializeField] private GameObject _gameOverUI;

    private Timer _timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _timer = FindObjectOfType<Timer>();

        Enemy.OnAnyEnemyDeath += HandleEnemyDeath;
    }

    private void OnDestroy()
    {
        Enemy.OnAnyEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        if (Enemy.TotalEnemiesAlive <= 0)
        {
            HandleLevelWin();
        }
    }

    public void HandleLevelWin()
    {
        if (_timer != null)
        {
            _timer.StopGameTimer();
        }

        Time.timeScale = 0f;

        if (_winnerUI != null)
        {
            _winnerUI.SetActive(true);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        if (_gameOverUI != null)
        {
            _gameOverUI.SetActive(true);
        }
    }
}
