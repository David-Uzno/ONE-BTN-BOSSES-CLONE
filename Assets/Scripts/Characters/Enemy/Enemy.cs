using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public delegate void EnemyDeathEventHandler(Enemy enemy);
    public event EnemyDeathEventHandler OnEnemyDeath;

    protected override void Die()
    {
        base.Die();

        if (OnEnemyDeath != null)
        {
            OnEnemyDeath(this);
        }
    }
}
