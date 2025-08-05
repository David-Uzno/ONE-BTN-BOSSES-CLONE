using UnityEngine;

public class EnemyCharacter : Character
{
    public delegate void EnemyDeathEventHandler(EnemyCharacter enemy);
    public static event EnemyDeathEventHandler OnAnyEnemyDeath;

    public static int TotalEnemiesAlive = 0;

    private void Awake()
    {
        TotalEnemiesAlive++;
    }

    protected override void Die()
    {
        base.Die();

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
