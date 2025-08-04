using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private int _initialHealth = 3;
    private int _health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    public void InitializeHealth()
    {
        _health = _initialHealth;
        UpdateLivesUI(_health);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        UpdateLivesUI(_health);

        if (_health <= 0)
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
