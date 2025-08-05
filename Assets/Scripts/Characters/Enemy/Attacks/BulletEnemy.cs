using UnityEngine;

public class BulletEnemy : Projectile
{
    [Header("StraightProjectile")]
    [SerializeField] private float _speedIncreaseFactor = 3f;
    private bool _hasIncreasedSpeed = false;

    private Vector2 _movementDirection;
    private CircularPath _circularPath;
    private HealthManager _healthManager;

    public void SetDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void SetCircularPath(CircularPath circularPath)
    {
        _circularPath = circularPath;
    }

    private void Update()
    {
        transform.Translate(_movementDirection * _speed * Time.deltaTime);
        CheckAndIncreaseSpeed();
    }

    private void CheckAndIncreaseSpeed()
    {
        if (_circularPath != null && !_hasIncreasedSpeed)
        {
            float distanceFromCenter = Vector2.Distance(transform.position, _circularPath.GetCenter());
            if (distanceFromCenter > _circularPath.GetRadius())
            {
                _speed *= _speedIncreaseFactor;
                _hasIncreasedSpeed = true;
            }
        }
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player") && _healthManager != null)
        {
            _healthManager.TakeDamage(1);
        }
    }
}
