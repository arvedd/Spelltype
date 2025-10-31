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
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        SpellData[] cards = Resources.LoadAll<SpellData>("Cards");
        allCards.AddRange(cards);

        AddCardToDeck("fireball");
        AddCardToDeck("terra");
        AddCardToDeck("ventus");
        AddCardToDeck("fireball");
        AddCardToDeck("curial");
        AddCardToDeck("waterball");
    }


    public void AddCardToDeck(string cardName)
    {
        SpellData cardToAdd = allCards.Find(c => c.spellName == cardName);
        if (cardToAdd != null)
        {
            PlayerDeck.Add(cardToAdd);
            Debug.Log($"Added {cardName} to deck!");
        }
        else
        {
            Debug.Log($"{cardName} does not exist");
        }
    }
        public void AddCard(SpellData cardToAdd)
    {
    if (cardToAdd != null)
    {
        PlayerDeck.Add(cardToAdd);
        Debug.Log($"✅ Added {cardToAdd.spellName} to player's deck (from Reward)!");
    }
    else
    {
        Debug.LogWarning("⚠️ Tried to add a null card to deck!");
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
            if (spell.spellName.ToLower() == spellName.ToLower())
            {
                return spell;
            }
        }
        return null;
    }
}
