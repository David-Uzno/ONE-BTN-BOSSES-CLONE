using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LinearProjectile : Projectile
{
    [Header("Projectile Lifetime")]
    [SerializeField] private float _lifetime = 5f; 

    private void Start()
    {
        _speed = 0;

        Destroy(gameObject, _lifetime);
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}


