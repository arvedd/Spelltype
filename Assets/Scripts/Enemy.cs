using System.Collections;
using UnityEngine;

public class Enemy : Damageable
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private SpellData spellData;
    [SerializeField] private Transform castPoint;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    

    void Start()
    {
        currentHP = enemy.enemyHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int dmg)
    {
        animator.SetTrigger("Hurt");
        base.TakeDamage(dmg);
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

    private IEnumerator FlashDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
