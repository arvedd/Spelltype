using System.Collections;
using UnityEngine;

public class FireSkullBehavior : EnemyBehavior
{
    public SpellData normalFireballData;
    public SpellData multiFireballData;

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

        int attackPhase = Random.Range(1, 2); // 0 = normal, 1 = multi
        if (attackPhase == 0)
            yield return NormalFireballAttack();
        else
            yield return MultiAttack();
    }

    private IEnumerator NormalFireballAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);

        GameObject obj = GameObject.Instantiate(normalFireballData.spellPrefab, castPoint.position, Quaternion.identity);
        Attack atk = obj.GetComponent<Attack>();

        if (atk != null)
        {
            atk.Initialize(normalFireballData.spellDamage, normalFireballData.spellSpeed, Vector2.left, Caster.Enemy);


            battle.RegisterEnemyAttack(atk);
        }
    }

    private IEnumerator MultiAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);

        int shotCount = 3;
        for (int i = 0; i < shotCount; i++)
        {
            GameObject obj = GameObject.Instantiate(multiFireballData.spellPrefab, castPoint.position, Quaternion.identity);
            Attack atk = obj.GetComponent<Attack>();

            if (atk != null)
            {
                atk.Initialize(multiFireballData.spellDamage, multiFireballData.spellSpeed, Vector2.left, Caster.Enemy);

                battle.RegisterEnemyAttack(atk);
            }

            if (i < shotCount - 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
