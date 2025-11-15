using System.Collections;
using UnityEngine;

public class CacodaemonBehavior : EnemyBehavior
{
    [Header("Spell Data")]
    public SpellData ChargeSpellData;
    public SpellData LaserSpellData;
    public SpellData MultiFireballData;

    [Header("Points")]
    public Transform castPoint;
    public Transform stunPoint;

    public GameObject StunEffect;


    [Header("Misc")]
    public float chargeTime = 7f;
    private Animator animator;
    private bool chargeInterrupted = false;
    private int attackPhase = 0;

    private void Start()
    {
        animator = enemy.GetComponent<Animator>();
        if (castPoint == null)
            castPoint = enemy.castPoint;
    }

    public override IEnumerator DoAttack()
    {
        if (animator == null)
            animator = enemy.GetComponent<Animator>();
        if (castPoint == null)
            castPoint = enemy.castPoint;

        // 0 = charge + laser, 1 = multi-fireball


        if (attackPhase == 2)
            yield return ChargeAndLaser();
        else
            yield return MultiFireballAttack();

        attackPhase++;

        if (attackPhase >= 3)
        attackPhase = 0;
        NotifyBattleSystemTurnEndCheck();
    }

    private IEnumerator ChargeAndLaser()
    {
        chargeInterrupted = false;

        // Start charging
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(0.3f);

        // Create counterable charge spell
        GameObject chargeObj = Instantiate(ChargeSpellData.spellPrefab, this.transform.position, Quaternion.identity);
        Attack chargeAtk = chargeObj.GetComponent<Attack>();
        if (chargeAtk != null)
        {
            AudioManager.Instance.PlaySpellSFX(ChargeSpellData, false);
            chargeAtk.Initialize(ChargeSpellData.spellDamage, ChargeSpellData.spellSpeed, Vector2.left, Caster.Enemy);
            battle.RegisterEnemyAttack(chargeAtk);
            Attack.OnAttackDestroyed += OnChargeDestroyed;
        }
        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            if (chargeInterrupted)
            {
                elapsed = 0f;
                Attack.OnAttackDestroyed -= OnChargeDestroyed;

                if (stunPoint != null)
                {
                    Instantiate(StunEffect, stunPoint.position, Quaternion.identity);
                    animator.Play("Hurt");
                }

                NotifyBattleSystemTurnEndCheck();
                yield return new WaitForSeconds(1.0f);
                yield break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Attack.OnAttackDestroyed -= OnChargeDestroyed;
        if (chargeObj != null)
            Destroy(chargeObj);
        yield return new WaitForSeconds(0.5f);

        GameObject laserObj = Instantiate(LaserSpellData.spellPrefab, castPoint.position, Quaternion.identity);
        AudioManager.Instance.PlaySpellSFX(LaserSpellData, false);
        
        float laserDuration = 1.0f;

        if (battle != null)
        {
            var player = FindAnyObjectByType<Player>();
            if (player != null)
            {
                player.TakeDamage(LaserSpellData.spellDamage);
            }
        }       
        yield return new WaitForSeconds(laserDuration);

        if (laserObj != null)
            Destroy(laserObj);

        animator.SetTrigger("ChargeEnd");
        NotifyBattleSystemTurnEndCheck();

    }

    private IEnumerator MultiFireballAttack()
    {
        
        yield return new WaitForSeconds(0.3f);

        int shotCount = 2;
        for (int i = 0; i < shotCount; i++)
        {
            animator.Play("Cacodaemon Attack");
            GameObject obj = Instantiate(MultiFireballData.spellPrefab, castPoint.position, Quaternion.identity);
            Attack atk = obj.GetComponent<Attack>();

            if (atk != null)
            {
                AudioManager.Instance.PlaySpellSFX(MultiFireballData, false);
                atk.Initialize(MultiFireballData.spellDamage, MultiFireballData.spellSpeed, Vector2.left, Caster.Enemy);
                battle.RegisterEnemyAttack(atk);
            }

            if (i < shotCount - 1)
                yield return new WaitForSeconds(0.4f);
        }

    }
    private void OnChargeDestroyed(Attack atk)
    {
        if (atk.spellData == ChargeSpellData)
        {
            chargeInterrupted = true;
            Attack.OnAttackDestroyed -= OnChargeDestroyed;
        }
    }

    public void NotifyBattleSystemTurnEndCheck()
    {
        if (battle != null)
            battle.CheckIfEnemyTurnShouldEnd();
    }
}
