using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Projectile
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player") && _gameManager != null)
        {
            _gameManager.TakeDamage(1);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
