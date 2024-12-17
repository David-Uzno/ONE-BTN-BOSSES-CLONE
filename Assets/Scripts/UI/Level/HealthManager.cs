using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private int initialHealth = 3;
    private int health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    public void InitializeHealth()
    {
        health = initialHealth;
        UpdateLivesUI(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateLivesUI(health);

        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void UpdateLivesUI(int currentHealth)
    {
        for (int i = 0; i < _lifeImages.Count; i++)
        {
            _lifeImages[i].enabled = i < currentHealth;
        }
    }
}
