using System.Collections;
using UnityEngine;

public class Enemy : Damageable
{
    public EnemyData enemy;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHP = enemy.enemyHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage(int dmg)
    {
        StartCoroutine(FlashDamage());
        base.TakeDamage(dmg);
    }

    public void SpellDamage(SpellData spell)
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

        TakeDamage(damage);

    }

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
