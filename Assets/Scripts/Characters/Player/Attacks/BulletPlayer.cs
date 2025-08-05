using UnityEngine;

public class BulletPlayer : Projectile
{
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
