using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player _player;

    [Header("Lives UI")]
    [SerializeField] private List<Image> _lifeImages;

    void Start()
    {
        if (_player != null)
        {
            _player.OnLifeLost += UpdateLivesUI;
        }
    }

    private void UpdateLivesUI(int currentHealth)
    {
        for (int i = 0; i < _lifeImages.Count; i++)
        {
            _lifeImages[i].enabled = i < currentHealth;
        }
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnLifeLost -= UpdateLivesUI;
        }
    }
}
