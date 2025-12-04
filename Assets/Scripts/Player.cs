using System.Collections;
using UnityEngine;

public class Player : Damageable
{
    public PlayerData player;
    public int currentAP;
    public int playerMaxHp;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (PlayerPrefs.HasKey("PlayerHP"))
        {
            currentHP = PlayerPrefs.GetInt("PlayerHP");
            Debug.Log($"Player punya pref ");
        }
        else
        {
            currentHP = player.playerMaxHealth;
            Debug.Log("Player ga punya pref");
        }

        playerMaxHp = player.playerMaxHealth;

        currentAP = player.attackPoin;
    }

    void OnDisable()
    {
        if (Ending.isResetting) return;

        if (currentHP == 0 )
        {
            PlayerPrefs.SetInt("PlayerHP", 100);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("PlayerHP", currentHP);
            PlayerPrefs.Save();
        }
    }

    public void UseItem(ShopData item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null!");
            return;
        }

        if (item.restoreHealth > 0)
        {
            int healAmount = Mathf.RoundToInt(item.restoreHealth * currentHP);
            Heal(healAmount);
            HealAnim();
            Debug.Log($"Player healed by {healAmount} HP using {item.itemName}");
        }

        if (item.restoreHealth > 0 && item.restoreAP > 0)
        {
            int healAmount = Mathf.RoundToInt(item.restoreHealth * currentHP);
            int apAmount = Mathf.RoundToInt(item.restoreAP);
            Heal(healAmount);
            RestoreAP(apAmount);
            HealAnim();
        }
        

    }

    public void UseAP(int cost)
    {
        currentAP -= cost;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, playerMaxHp);
    }

    public void RestoreAP(int amount)
    {
        if (currentAP < 5)
        {
            currentAP += amount;
        }
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        animator.SetTrigger("Hurt");

    }

    public override void Die()
    {
        DeadAnim();
        base.Die();
    }

    public void AttackAnim()
    {
        animator.SetTrigger("Attack");
    }

    public void HealAnim()
    {
        animator.SetTrigger("Heal");
    }

    public void DeadAnim()
    {
        animator.SetTrigger("Dead");
    }

    public void WinAnim()
    {
        animator.SetTrigger("Win");
    }

    
}
