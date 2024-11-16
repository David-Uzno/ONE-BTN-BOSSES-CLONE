using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProyectile : Projectile
{
    private void Start()
    {
        _speed = 0;
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            Destroy(other.gameObject);
          
        }
    }
}

