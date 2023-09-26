using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float ActualHealth { get; set; }
    public bool HasEnemy { get; }

    public event Action Death;

    public void GetDamage(float rangeOfDamage);
    public void Die();
    public void RestartAfterDie();
}
