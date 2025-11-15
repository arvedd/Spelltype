using System.Collections;
using UnityEngine;

public class AmanitaBehaviour : EnemyBehavior
{
    public SpellData normalVentusData;
    public SpellData multiVentusData;

    private Animator animator;
    private Transform castPoint;

    private void Start()
    {
        animator = enemy.GetComponent<Animator>();
        castPoint = enemy.castPoint;
    }

    public override IEnumerator DoAttack()
    {
        if (animator == null)
            animator = enemy.GetComponent<Animator>();

        if (castPoint == null)
            castPoint = enemy.castPoint;

        // Cek max attack berdasarkan enemy type
        int maxAttacks = enemy.GetMaxAttacksPerTurn();

        // Jika enemy biasa (maxAttacks = 1), selalu gunakan normal attack
        if (maxAttacks == 1)
        {
            yield return NormalAttack();
        }
        else
        {
            // Elite/Boss bisa pakai multi attack
            int attackPhase = Random.Range(0, 2); // 0 = normal, 1 = multi
            if (attackPhase == 0)
                yield return NormalAttack();
            else
                yield return MultiAttack();
        }
    }

    private IEnumerator NormalAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);

        GameObject obj = GameObject.Instantiate(normalVentusData.spellPrefab, castPoint.position, Quaternion.identity);
        Attack atk = obj.GetComponent<Attack>();

        if (atk != null)
        {
            AudioManager.Instance.PlaySpellSFX(normalVentusData, false);
            atk.Initialize(normalVentusData.spellDamage, normalVentusData.spellSpeed, Vector2.left, Caster.Enemy);
            battle.RegisterEnemyAttack(atk);
        }
    }

    private IEnumerator MultiAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);

        // Jumlah shot dibatasi oleh max attacks enemy
        int maxAttacks = enemy.GetMaxAttacksPerTurn();
        int shotCount = Mathf.Min(3, maxAttacks); // Max 3 shot, tapi dibatasi enemy type
        
        for (int i = 0; i < shotCount; i++)
        {
            GameObject obj = GameObject.Instantiate(multiVentusData.spellPrefab, castPoint.position, Quaternion.identity);
            Attack atk = obj.GetComponent<Attack>();

            if (atk != null)
            {
                AudioManager.Instance.PlaySpellSFX(multiVentusData, false);
                atk.Initialize(multiVentusData.spellDamage, multiVentusData.spellSpeed, Vector2.left, Caster.Enemy);
                battle.RegisterEnemyAttack(atk);
            }

            if (i < shotCount - 1)
            {
                yield return new WaitForSeconds(0.5f);
            }     
        }
    }
}
