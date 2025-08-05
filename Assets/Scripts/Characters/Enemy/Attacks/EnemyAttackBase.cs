using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    protected HealthManager _healthManager;

    protected virtual void Awake()
    {
        _healthManager = Object.FindFirstObjectByType<HealthManager>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}