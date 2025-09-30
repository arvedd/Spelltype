using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<SpellData> allCards = new List<SpellData>();

    private int currentIndex = 0;
    public int startingHandSize = 4;
    public int maxHandSize;
    public int currentHandSize;

    private HandManager handManager;

    void Start()
    {
        SpellData[] cards = Resources.LoadAll<SpellData>("Cards");

        allCards.AddRange(cards);

        handManager = FindFirstObjectByType<HandManager>();
        maxHandSize = handManager.maxHandSize;

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
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
            return;


        if (currentHandSize < maxHandSize)
        {
            SpellData nextCard = allCards[currentIndex];
            handManager.AddCardsToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }
}
