using UnityEngine;

public class BulletEnemy : Projectile
{
    [Header("Projectile")]
    [SerializeField] private float _speedIncreaseFactor = 3f;

    [Header("Movement")]
    private Vector2 _movementDirection;
    private CircularPath _circularPath;

    [Header("Health Management")]
    private HealthManager _healthManager;

    private bool _hasIncreasedSpeed = false;

    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void AssignCircularPath(CircularPath circularPath)
    {
        _circularPath = circularPath;
    }

    private void Update()
    {
        MoveProjectile();
        TryIncreaseSpeed();
    }

    private void MoveProjectile()
    {
        transform.Translate(_movementDirection * _speed * Time.deltaTime);
    }

    private void TryIncreaseSpeed()
    {
        if (_circularPath == null || _hasIncreasedSpeed) return;

        float distanceFromCenter = Vector2.Distance(transform.position, _circularPath.GetCenter());
        if (distanceFromCenter > _circularPath.GetRadius())
        {
            _speed *= _speedIncreaseFactor;
            _hasIncreasedSpeed = true;
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
