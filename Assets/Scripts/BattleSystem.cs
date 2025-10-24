using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject objectPlayer;
    public GameObject objectEnemy;
    public TextMeshProUGUI turnText;
    public Transform playerBattleSpot;
    public Transform enemyBattleSpot;
    public SpellTyper playerTyper;
    public HandManager handManager;
    public DeckManager deckManager;

    [SerializeField] private Enemy enemyData;
    [SerializeField] private Player playerData;


    void Start()
    {
        handManager = FindAnyObjectByType<HandManager>();
        deckManager = FindAnyObjectByType<DeckManager>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        
    }

    IEnumerator SetupBattle()
    {
        GameObject playerInstance = Instantiate(objectPlayer, playerBattleSpot);
        GameObject enemyInstance = Instantiate(objectEnemy, enemyBattleSpot);

        playerTyper = playerInstance.GetComponentInChildren<SpellTyper>();
        playerTyper.OnPlayerFinished += EndPlayerTurn;

        playerData = playerInstance.GetComponentInChildren<Player>();
        enemyData = enemyInstance.GetComponentInChildren<Enemy>();

        playerTyper.enabled = false;

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void EnemyTurn()
    {
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("It's Enemy Turn");
        turnText.text = "Enemy Turn";

        yield return new WaitForSeconds(0.5f);

        enemyData.CastSpell();

        yield return new WaitForSeconds(2f);

        turnText.text = "";
        EndEnemyTurn();
    }

    private void PlayerTurn()
    {
        for (int i = 0; i < deckManager.maxHandSize; i++)
        {
            deckManager.DrawCard(handManager);
        }

        CheckIfDied();
        playerTyper.enabled = true;
        playerData.currentAP = playerData.player.attackPoin;
        turnText.text = "Player Turn";
    }

    private void EndEnemyTurn()
    {
        CheckIfDied();
        if (state != BattleState.WON && state != BattleState.LOST)
        {
            state = BattleState.PLAYERTURN;
            turnText.text = "";
            PlayerTurn();
        }
    }

    private void EndPlayerTurn()
    {
        CheckIfDied();

        if (handManager.cardsInHand.Count > 0) 
        {
            List<GameObject> cardsToDiscard = new List<GameObject>(handManager.cardsInHand);

            foreach (GameObject cardObj in cardsToDiscard)
            {
                deckManager.DiscardCard(cardObj, handManager);
            }

            handManager.UpdateHandVisual();
        }

        if (state != BattleState.WON && state != BattleState.LOST)
        {
            state = BattleState.ENEMYTURN;
            playerTyper.enabled = false;
            turnText.text = "";
            EnemyTurn();
        }

  
    }

    public void CheckIfDied()
    {
        if (enemyData.currentHP <= 0)
        {
            state = BattleState.WON;
            turnText.text = "YOU WIN!";
            return;
        }
        
        if(playerData.currentHP <= 0)
        {
            state = BattleState.LOST;
            turnText.text = "YOU LOST!";
            return;
        }
    }

}
