using System.Collections;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    protected Enemy enemy;
    protected BattleSystem battle;

    public void Initialize(Enemy e, BattleSystem b)
    {
        enemy = e;
        battle = b;
    }

    public abstract IEnumerator DoAttack();
}
