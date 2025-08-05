using UnityEngine;

public class BulletPlayer : Projectile
{
    private ProjectilePool _projectilePool;

    private void Awake()
    {
        _projectilePool = Object.FindAnyObjectByType<ProjectilePool>();
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (IsEnemy(other))
        {
            DamageEnemy(other);
        }

        _projectilePool.ReturnBulletToPool(transform);
    }

    private bool IsEnemy(Collider2D collider)
    {
        return collider.CompareTag("Enemy");
    }

    private void DamageEnemy(Collider2D collider)
    {
        EnemyCharacter enemy = collider.GetComponent<EnemyCharacter>();

        if (enemy != null)
        {
            enemy.TakeDamage(1);
        }
    }
}
