using UnityEngine;

public class BulletEnemy : Projectile
{
    private Vector2 _direction;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colisión Contra el Jugador");
        }
    }
}
