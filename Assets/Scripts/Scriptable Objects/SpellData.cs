using UnityEngine;

public enum TypeOfSpell {Fire, Water, Wind, Earth};

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public int spellDamage;
    public float spellSpeed;
    public int spellCost;
    public Sprite elementImage;
    public TypeOfSpell typeSpell;
    public GameObject spellPrefab;
}
