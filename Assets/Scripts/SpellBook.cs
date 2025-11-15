using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public static SpellBook Instance;

    [Header("Unlocked Spells")]
    public List<SpellData> unlockedSpells = new List<SpellData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void UnlockSpell(SpellData spell)
    {
        if (!unlockedSpells.Contains(spell))
        {
            unlockedSpells.Add(spell);
            Debug.Log($"✨ Added to SpellBook: {spell.spellName}");
        }
    }

    public bool HasSpell(string spellName)
    {
        return unlockedSpells.Exists(s => s.spellName.Equals(spellName, System.StringComparison.OrdinalIgnoreCase));
    }

    public SpellData GetSpell(string spellName)
    {
        return unlockedSpells.Find(s => s.spellName.Equals(spellName, System.StringComparison.OrdinalIgnoreCase));
    }
}
