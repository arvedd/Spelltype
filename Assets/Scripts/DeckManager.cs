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

    
    [SerializeField] private TextMeshProUGUI Drawpiletext;
    [SerializeField] private TextMeshProUGUI Discardpiletext;

    void Start()
    {
        playerDeckManager = FindAnyObjectByType<PlayerDeckManager>();

        drawPile = new List<SpellData>(playerDeckManager.PlayerDeck);
        Shuffle(drawPile);

        handManager = FindFirstObjectByType<HandManager>();
        maxHandSize = handManager.maxHandSize;

        Discardpiletext.text = discardPile.Count.ToString();
        Drawpiletext.text = drawPile.Count.ToString(); 

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

            Discardpiletext.text = discardPile.Count.ToString();
            Drawpiletext.text = drawPile.Count.ToString();

            Shuffle(drawPile);
        }

        if (handManager.cardsInHand.Count < maxHandSize)
        {
            SpellData nextCard = drawPile[0];
            drawPile.RemoveAt(0);

            handManager.AddCardsToHand(nextCard);
        }
        else
        {
            Debug.Log("Cannot draw: hand is full!");
        }

        Drawpiletext.text = drawPile.Count.ToString();
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

    public void DiscardCard(GameObject cardObj, HandManager handManager)
    {
        if (cardObj == null) return;

        CardDispay cardDisplay = cardObj.GetComponent<CardDispay>();
        if (cardDisplay != null)
        {
            SpellData data = cardDisplay.spellData;
            discardPile.Add(data);
        }

        // Remove from hand list
        if (handManager.cardsInHand.Contains(cardObj))
            handManager.cardsInHand.Remove(cardObj);

        // Destroy the GameObject
        Destroy(cardObj);

        // Update UI if assigned
        if (Discardpiletext != null)
            Discardpiletext.text = discardPile.Count.ToString();

        // Update hand visuals
        handManager.UpdateHandVisual();
    }


}
