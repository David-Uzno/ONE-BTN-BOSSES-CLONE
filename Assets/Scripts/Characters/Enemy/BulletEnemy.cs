using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : Projectile
{
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(1);
            }
            
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
