using System;
using System.Collections;
using UnityEngine;

public enum EnemyType 
{ 
    Normal,    // Enemy biasa - 1 attack per turn
    Elite,     // Elite enemy - bisa multiple attacks
    Boss       // Boss enemy - bisa multiple attacks
}

public class Enemy : Damageable
{
    [SerializeField] private EnemyData enemy;
    [SerializeField] private SpellData spellData;
    [SerializeField] public Transform castPoint;
    [SerializeField] private Healthbar healthbar;
    public int enemyMaxHp;
    public Animator animator;
    public event Action<Enemy> OnEnemyDeath;

    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Normal;

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

    public override void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        OnEnemyDeath?.Invoke(this);

        // Play death animation
        animator.SetTrigger("Death");

        // Hancurkan object setelah animasi selesai
        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(1f); // durasi animasi Death
        Destroy(gameObject);
    }


    private bool AnimatorHasParameter(Animator anim, string paramName, AnimatorControllerParameterType type)
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == type && param.name == paramName)
                return true;
        }
        return false;
    }

    private void OnDeathEnd()
    {
        Destroy(gameObject);
    }

    public Attack CastSpell()
    {
        animator.SetTrigger("Attack");
        GameObject obj = Instantiate(spellData.spellPrefab, castPoint.position, Quaternion.identity);

        Attack atk = obj.GetComponent<Attack>();
        if (atk != null)
        {
            
            Vector2 dir = Vector2.left;
            atk.Initialize(spellData.spellDamage, spellData.spellSpeed, dir, Caster.Enemy);
            return atk;
        }

        return null;
    }

    public IEnumerator DoTurn(BattleSystem battle)
    {
        EnemyBehavior behavior = GetComponent<EnemyBehavior>();

        if (behavior != null)
        {
            behavior.Initialize(this, battle);
            yield return behavior.DoAttack();
        }
        else
        {
            Attack atk = CastSpell();
            if (atk != null)
            {
                battle.RegisterEnemyAttack(atk);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public int GetMaxAttacksPerTurn()
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                return 1;
            case EnemyType.Elite:
                return 1;
            case EnemyType.Boss:
                return 1;
            default:
                return 1;
        }
    }


}
