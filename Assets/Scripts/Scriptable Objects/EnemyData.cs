using UnityEngine;

public enum EnemyName { Goblin, Orc, Lich }

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyName enemyName;
    public TypeOfSpell enemyWeakness;
    public int enemyHp;
}
