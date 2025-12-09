using UnityEngine;

public class BulletPlayer : Projectile
{
    private ObjectPool _projectilePool;

    private void Awake()
    {
        _projectilePool = FindAnyObjectByType<ObjectPool>();
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (IsEnemy(other))
        {
            DamageEnemy(other);
        }

        _projectilePool.ReturnObjectToPool(transform);
    }

    private bool IsEnemy(Collider2D collider)
    {
        return collider.CompareTag("Enemy");
    }

    private void DamageEnemy(Collider2D collider)
    {
        if (collider.TryGetComponent(out EnemyCharacter enemy))
        {
            enemy.TakeDamage(1);
        }
    }
}
