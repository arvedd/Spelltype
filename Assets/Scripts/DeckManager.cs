using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    
    public List<SpellData> drawPile = new List<SpellData>();
    public List<SpellData> discardPile = new List<SpellData>();
    public PlayerDeckManager playerDeckManager;

    // private int currentIndex = 0;
    public int startingHandSize = 4;
    public int maxHandSize;
    public int currentHandSize;

    private HandManager handManager;

    
    //[SerializeField] private TextMeshProUGUI Drawpiletext;
    //[SerializeField] private TextMeshProUGUI Discardpiletext;

    void Start()
    {
        playerDeckManager = FindAnyObjectByType<PlayerDeckManager>();

        drawPile = new List<SpellData>(playerDeckManager.PlayerDeck);
        Shuffle(drawPile);

        handManager = FindFirstObjectByType<HandManager>();
        maxHandSize = handManager.maxHandSize;

        // Draw starting hand
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }


    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }

        if (currentHandSize != startingHandSize)
        {
            DrawCard(handManager);
        }

       // Drawpiletext.text = drawPile.Count.ToString();
       // Discardpiletext.text = discardPile.Count.ToString();
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0)
                return; // No cards left at all

            // Reshuffle discard into draw pile
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
            Debug.Log("Reshuffled discard pile back into deck.");
        }

        if (handManager.cardsInHand.Count < maxHandSize)
        {
            SpellData nextCard = drawPile[0];
            drawPile.RemoveAt(0);

            handManager.AddCardsToHand(nextCard);
        }
    }

    private void Shuffle(List<SpellData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            SpellData temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void DiscardCard(SpellData card)
    {
        discardPile.Add(card);
        Debug.Log($"Discarded {card.spellName}");
    }

}
