using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour, IDamageable
{
    [Header("Player Status")]
    [SerializeField] private int _health = 3;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        UpdateLivesUI(_health);

        if (_health <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
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
