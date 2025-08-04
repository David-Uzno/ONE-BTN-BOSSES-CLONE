using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private int _health = 10;

    public void SetHealth(int health)
    {
        _health = health;
    }

    public virtual void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
