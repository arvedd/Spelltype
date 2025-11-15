using UnityEngine;

public enum TypeOfSpell {Fire, Water, Wind, Earth, Lightning, Heal};
public enum SpellCategory { Attack, Heal, Defend, Strike }

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public int spellDamage;
    public float spellSpeed;
    public int spellCost;
    public int spellHeal;
    public Sprite cardImage;
    public Sprite elementImage;
    public Sprite cardBadge;
    public Sprite cardAP;
    public TypeOfSpell typeSpell;
    public SpellCategory spellCategory;
    public GameObject spellPrefab;
    public AudioClip castSFX;   
    public AudioClip impactSFX;

    [Header("Counter Spell Info")]
    [Tooltip("Words the player can type to counter this spell")]
    public string[] counterWords;
}
