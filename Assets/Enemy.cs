using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemy;
    private int currentHP;

    void Start()
    {
        currentHP = enemy.enemyHp;
    }

    public void TakeDamage(SpellData spell)
    {
        int damage = spell.spellDamage;

        if (spell.typeSpell == enemy.enemyWeakness)
        {
            Debug.Log(enemy.enemyName + " is weak to " + spell.typeSpell);
        }
        else
        {
            damage /= 2;
            Debug.Log(enemy.enemyName + " is not weak to " + spell.typeSpell + "! Half damage");
        }

        currentHP -= damage;
        Debug.Log(enemy.enemyName + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log(enemy.enemyName + " Destroyed!");
        Destroy(gameObject);
    }
}
