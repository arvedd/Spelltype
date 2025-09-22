using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "ScriptableObjects/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public int spellDamage;
    public float spellSpeed;
    public GameObject spellPrefab;
}
