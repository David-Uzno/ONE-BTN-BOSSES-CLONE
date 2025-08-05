using UnityEngine;

public class ObstacleInteraction
{
    private HealthManager _healthManager;

    public ObstacleInteraction(HealthManager healthManager)
    {
        _healthManager = healthManager;
    }

    public void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}