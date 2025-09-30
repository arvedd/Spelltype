using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab;
    public Transform handTransform;
    public float handSpread = 7.5f;
    public float cardSpacing = 100f;
    public float verticalSpacing = 10f;
    public int maxHandSize = 4;
    public List<GameObject> cardsInHand = new List<GameObject>();

    void Start()
    {
     
    }

    public void AddCardsToHand(SpellData spellData)
    {
        if (cardsInHand.Count < maxHandSize)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            newCard.GetComponent<CardDispay>().spellData = spellData;
        }

        UpdateHandVisual();
    }

    public void OnPlayerTyped(string inputText)
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            CardDispay card = cardsInHand[i].GetComponent<CardDispay>(); 
            SpellData data = card.spellData;                  

            if (card != null && data.spellName.Equals(inputText, System.StringComparison.OrdinalIgnoreCase))
            {
                GameObject cardToRemove = cardsInHand[i];
                cardsInHand.RemoveAt(i);
                Destroy(cardToRemove);
                break;
            }
        }

        UpdateHandVisual();
    }

    private void UpdateHandVisual()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);

            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (handSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticallOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticallOffset, 0f);

        }
    }


}
