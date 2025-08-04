using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField ]private Vector2 _direction;

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    protected abstract void HandleCollision(Collider2D other);
}
