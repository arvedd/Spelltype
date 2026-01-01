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
        if (spell == null)
        {
            Debug.LogWarning("Tried to unlock null spell!");
            return;
        }

        if (!unlockedSpells.Contains(spell))
        {
            unlockedSpells.Add(spell);
            Debug.Log($"Added to SpellBook: {spell.spellName}");

            
            if (PlayerDeckManager.instance != null)
            {
                PlayerDeckManager.instance.AddUnlockedSpellToDeck(spell);
            }
            else
            {
                Debug.LogWarning("PlayerDeckManager instance not found!");
            }
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
    public void ResetSpellBook()
    {
        unlockedSpells.Clear();
        Debug.Log("SpellBook has been reset!");
    }

}
