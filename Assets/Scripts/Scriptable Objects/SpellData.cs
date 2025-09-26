using UnityEngine;

public enum TypeOfSpell {Fire, Water, Leaf};

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public int spellDamage;
    public float spellSpeed;
    public TypeOfSpell typeSpell;
    public GameObject spellPrefab;
}
