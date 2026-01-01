using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    public List<SpellData> PlayerDeck = new List<SpellData>();
    public List<SpellData> allCards = new List<SpellData>();

    
    public static PlayerDeckManager instance;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
           
            InitializeAllCards();
            
            
            InitializeDefaultDeck();
            SyncWithSpellBook();
        }
        else
        {
            
            Destroy(gameObject);
        }
    }

    
    
    private void InitializeAllCards()
    {
        SpellData[] cards = Resources.LoadAll<SpellData>("Cards");
        allCards.AddRange(cards);
    }

    public void InitializeDefaultDeck()
    {
        if (PlayerDeck.Count > 0)
        {
            return;
        }

        AddCardToDeck("fireball");
        AddCardToDeck("terra");
        AddCardToDeck("ventus");
        AddCardToDeck("waterball");
        AddCardToDeck("curial");
       
        
        Debug.Log("Deck default telah diinisialisasi.");
    }
    

    public void AddCardToDeck(string cardName)
    {
        SpellData cardToAdd = allCards.Find(c => c.spellName.Equals(cardName, System.StringComparison.OrdinalIgnoreCase));
        if (cardToAdd != null)
        {
            PlayerDeck.Add(cardToAdd);
        }
        else
        {
            Debug.LogWarning($"{cardName} ga ada di allCards list!");
        }
    }
    
    public void AddUnlockedSpellToDeck(SpellData spell)
    {
        if (spell == null)
        {
            Debug.LogWarning("Spell null, ga bisa ditambahkan ke deck.");
            return;
        }

        if (PlayerDeck.Contains(spell))
        {
            Debug.Log($"Spell {spell.spellName} sudah ada di deck.");
            return;
        }

        PlayerDeck.Add(spell);
        Debug.Log($"Spell {spell.spellName} ditambahkan ke deck dari SpellBook!");
    }


    private void SyncWithSpellBook()
    {
        if (SpellBook.Instance == null) return;

        foreach (SpellData spell in SpellBook.Instance.unlockedSpells)
        {
            if (!PlayerDeck.Contains(spell))
            {
                PlayerDeck.Add(spell);
            }
        }
    }


    [System.Serializable]
    public class DeckSaveData
    {
        public List<string> cardNames;
        public DeckSaveData(List<string> names) { cardNames = names; }
    }

    public SpellData findspell(string spellName)
    {
       foreach (SpellData spell in allCards)
        {
            if (spell.spellName.Equals(spellName, System.StringComparison.OrdinalIgnoreCase))
            {
                return spell;
            }
        }
        return null;
    }
}