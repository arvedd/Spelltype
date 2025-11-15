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
        Debug.Log($"Loaded {allCards.Count} total SpellData from Resources.");
    }

    public void InitializeDefaultDeck()
    {
        if (PlayerDeck.Count > 0)
        {
            Debug.Log("Deck sudah ada. Melewati inisialisasi default.");
            return;
        }

        AddCardToDeck("fireball");
        AddCardToDeck("terra");
        AddCardToDeck("ventus");
        AddCardToDeck("waterball");
        AddCardToDeck("curial");
        AddCardToDeck("waterblast");
        AddCardToDeck("infernoblast");
        
        
        Debug.Log("Deck default telah diinisialisasi.");
    }
    

    public void AddCardToDeck(string cardName)
    {
        SpellData cardToAdd = allCards.Find(c => c.spellName.Equals(cardName, System.StringComparison.OrdinalIgnoreCase));
        if (cardToAdd != null)
        {
            PlayerDeck.Add(cardToAdd);
            Debug.Log($"Added {cardName} to deck! Total cards: {PlayerDeck.Count}");
        }
        else
        {
            Debug.LogWarning($"{cardName} does not exist in allCards list!");
        }
    }
    
    public void AddCard(SpellData cardToAdd)
    {
        if (cardToAdd != null)
        {
            PlayerDeck.Add(cardToAdd);
            Debug.Log($"Added {cardToAdd.spellName} to player's deck (from Reward)! Total cards: {PlayerDeck.Count}");
        }
        else
        {
            Debug.LogWarning("Tried to add a null card to deck!");
        }
    }

    public void LoadDeck()
    {
        string json = PlayerPrefs.GetString("PlayerDeck", "");
        if (!string.IsNullOrEmpty(json))
        {
            DeckSaveData data = JsonUtility.FromJson<DeckSaveData>(json);
            PlayerDeck.Clear();
            foreach (string name in data.cardNames)
                AddCardToDeck(name);
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