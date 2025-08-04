using UnityEngine;

public class BulletEnemy : Projectile
{
    private HealthManager _healthManager;

    private void Awake()
    {
        _healthManager = Object.FindFirstObjectByType<HealthManager>();
    }
    
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
