using UnityEngine;

public enum TypeOfSpell {Fire, Water, Wind, Earth, Heal};
public enum SpellCategory { Attack, Heal, Defend }

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public int spellDamage;
    public float spellSpeed;
    public int spellCost;
    public int spellHeal;
    public Sprite elementImage;
    public TypeOfSpell typeSpell;
    public SpellCategory spellCategory;
    public GameObject spellPrefab;

    [Header("Counter Spell Info")]
    [Tooltip("Words the player can type to counter this spell")]
    public string[] counterWords;
}
