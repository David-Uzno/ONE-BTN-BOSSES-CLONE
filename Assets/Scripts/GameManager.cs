using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Varialbes
    [Header("Player Status")]
    [SerializeField] private GameObject playerReference;
    [SerializeField] private int initialHealth = 3;
    private int health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    [Header("UI Results")]
    [SerializeField] private GameObject winnerUI;
    [SerializeField] private GameObject gameOverUI;
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
        Time.timeScale = 0f;
        winnerUI.SetActive(true);
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        Destroy(playerReference.gameObject);

        gameOverUI.SetActive(true);
    }
    #endregion
}
