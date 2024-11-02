using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Status Player")]
    [SerializeField] private MovementPlayer _player;
    [SerializeField] private int _initialHealth = 3;
    private int _health;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;


    void Start()
    {
        _health = _initialHealth;

        if (_player != null)
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
            Destroy(_player.gameObject);
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
