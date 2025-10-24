using System.Collections;
using UnityEngine;

public class Enemy : Damageable
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private SpellData spellData;
    [SerializeField] private Transform castPoint;
    [SerializeField] private Healthbar healthbar;
    public int enemyMaxHp;
    private Animator animator;


    void Start()
    {
        healthbar = GetComponentInChildren<Healthbar>();
        animator = GetComponent<Animator>();

        currentHP = enemy.enemyHealth;
        enemyMaxHp = enemy.enemyMaxHealth;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        animator.SetTrigger("Hurt");
        healthbar.UpdateHealthBar(currentHP, enemyMaxHp);
    }

    public void SpellDamage(SpellData spell)
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

        TakeDamage(damage);

    }

    public void CastSpell()
    {
        animator.SetTrigger("Attack");
        GameObject obj = Instantiate(spellData.spellPrefab, castPoint.position, Quaternion.identity);

        Attack atk = obj.GetComponent<Attack>();
        if (atk != null)
        {

            Vector2 dir = Vector2.left;
            atk.Initialize(spellData.spellDamage, spellData.spellSpeed, dir, Caster.Enemy);
        }
        
    }
}
