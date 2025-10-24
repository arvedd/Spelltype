using System.Collections;
using UnityEngine;

public class Player : Damageable
{
    public PlayerData player;
    public int currentAP;
    public int playerMaxHp;
    private Animator animator;
    [SerializeField] private Healthbar healthbar;


    void Start()
    {
        healthbar = GetComponentInChildren<Healthbar>();
        currentHP = player.playerHealth;
        playerMaxHp = player.playerMaxHealth;
        healthbar.UpdateHealthBar(currentHP, playerMaxHp);

        currentAP = player.attackPoin;
        animator = GetComponent<Animator>();
    }

    public void UseAP(int cost)
    {
        currentAP -= cost;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, playerMaxHp);
        healthbar.UpdateHealthBar(currentHP, playerMaxHp);
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        animator.SetTrigger("Hurt");
        healthbar.UpdateHealthBar(currentHP, playerMaxHp);
        
    }

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }

    public void HealAnim()
    {
        animator.SetTrigger("Heal");
    }

    
}
