using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public GameObject player;
    public GameObject enemy;
    public TextMeshProUGUI turnText;
    public Transform playerBattleSpot;
    public Transform enemyBattleSpot;
    public SpellTyper playerTyper;

    void OnEnable()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        playerTyper.OnPlayerFinished += EndPlayerTurn;
    }

    void OnDisable()
    {
        playerTyper.OnPlayerFinished -= EndPlayerTurn;
    }

    IEnumerator SetupBattle()
    {
        GameObject playerInstance = Instantiate(player, playerBattleSpot);
        GameObject enemyInstance = Instantiate(enemy, enemyBattleSpot);

        playerTyper = playerInstance.GetComponentInChildren<SpellTyper>();

        playerTyper.enabled = false;

        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void EnemyTurn()
    {
        Debug.Log("It's Enemy Turn");
    }

    private void PlayerTurn()
    {
        playerTyper.enabled = true;
    }

    private void EndPlayerTurn()
    {
        state = BattleState.ENEMYTURN;
        playerTyper.enabled = false;
        EnemyTurn();
    }

}
