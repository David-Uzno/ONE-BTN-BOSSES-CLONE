using UnityEngine;

public class EnemyCharacter : MonoBehaviour, IDamageable
{
    [Header("Status")]
    [SerializeField] private int _health = 10;

    public delegate void EnemyDeathEventHandler(EnemyCharacter enemy);
    public static event EnemyDeathEventHandler OnAnyEnemyDeath;

    public static int TotalEnemiesAlive = 0;

    private void Awake()
    {
        TotalEnemiesAlive++;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        TotalEnemiesAlive--;

        if (OnAnyEnemyDeath != null)
        {
            OnAnyEnemyDeath.Invoke(this);
        }

        if (TotalEnemiesAlive <= 0)
        {
            GameManager.Instance.HandleLevelWin();
        }
    }

    private void OnDestroy()
    {
        if (TotalEnemiesAlive > 0)
        {
            TotalEnemiesAlive--;
        }
    }
}
