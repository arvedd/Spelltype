using System.Collections;
using UnityEngine;

public class Player : Damageable
{
    public PlayerData player;
    public int currentAP;
    private Animator animator;

    void Start()
    {
        currentHP = player.playerHealth;
        currentAP = player.attackPoin;
        animator = GetComponent<Animator>();
    }

    public void UseAP(int cost)
    {
        currentAP -= cost;
    }

    public override void TakeDamage(int dmg)
    {
        animator.SetTrigger("Hurt");
        base.TakeDamage(dmg);
    }
    
    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }

    
}
