using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int currentHP;

    public virtual void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
        FindAnyObjectByType<BattleSystem>().CheckIfDied();
    }
}
