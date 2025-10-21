using System.Collections.Generic;
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
}
