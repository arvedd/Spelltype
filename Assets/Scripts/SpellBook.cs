using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    public List<SpellData> unlockedSpell = new List<SpellData>();

    public void UnlockSpell(SpellData spell)
    {
        if (!unlockedSpell.Contains(spell))
        {
            unlockedSpell.Add(spell);
            Debug.Log($"Unlocked: {spell.spellName}");
        }
        else
        {
            Debug.Log($"{spell.spellName} sudah kamu punya!");
        }
    }

    public bool HasSpell(string spellName)
    {
        foreach (SpellData spell in unlockedSpell)
        {
            if (spell.spellName.ToLower() == spellName.ToLower())
            {
                return true;
            }
        }
        return false;
    }
    
    public SpellData GetSpell(string spellName)
    {
        foreach (SpellData spell in unlockedSpell)
        {
            if (spell.spellName.ToLower() == spellName.ToLower())
            {
                return spell;
            }
        }   
        return null;
    }
}
