using UnityEngine;

public class BulletEnemy : Projectile
{
    [Header("Projectile")]
    [SerializeField] private float _speedIncreaseFactor = 3f;

    [Header("Movement")]
    private Vector2 _movementDirection;
    private ILevelLayout _layout;
    private IRadialLayout _radialLayout;

    [Header("Health Management")]
    private HealthManager _healthManager;

    private bool _hasIncreasedSpeed = false;

    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }

    public void AssignLayout(ILevelLayout layout)
    {
        _layout = layout;
        _radialLayout = layout as IRadialLayout;
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
        if (_layout == null || _hasIncreasedSpeed || _radialLayout == null) return;

        float distanceFromCenter = Vector2.Distance(transform.position, _layout.GetCenter());
        if (distanceFromCenter > _radialLayout.Radius)
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
