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
        if (other.CompareTag("Enemy"))
        {
            EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();

            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            _projectilePool.ReturnBullet(transform);
        }
        else
        {
            _projectilePool.ReturnBullet(transform);
        }
    }
}
