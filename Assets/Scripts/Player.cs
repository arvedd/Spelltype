using UnityEngine;

public class Player : Damageable
{
    public PlayerData player;
    public int currentAP;

    void Start()
    {
        currentHP = player.playerHealth;
        currentAP = player.attackPoin;
    }

    public void UseAP(int cost)
    {
        currentAP -= cost;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }
}
