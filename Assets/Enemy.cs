using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemy;
    private int currentHP;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHP = enemy.enemyHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(SpellData spell)
    {
        int damage = spell.spellDamage;
        StartCoroutine(FlashDamage());

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

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        Debug.Log(enemy.enemyName + " Destroyed!");
        Destroy(gameObject);
    }
}
