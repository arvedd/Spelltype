using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    
    [Header("Player")]
    public GameObject objectPlayer;
    public Transform playerBattleSpot;
    public SpellTyper playerTyper;
    private Player playerData;
    
    [Header("Enemy Spawner")]
    public EnemySpawner enemySpawner;
    private List<Enemy> enemies = new List<Enemy>();
    private Enemy currentTargetEnemy;
    private int currentEnemyIndex = 0;
    
    [Header("UI")]
    public TextMeshProUGUI turnText;
    
    [Header("Managers")]
    public HandManager handManager;
    public DeckManager deckManager;

    void Start()
    {
        handManager = FindAnyObjectByType<HandManager>();
        deckManager = FindAnyObjectByType<DeckManager>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        // Spawn player
        GameObject playerInstance = Instantiate(objectPlayer, playerBattleSpot);
        playerTyper = playerInstance.GetComponentInChildren<SpellTyper>();
        playerTyper.OnPlayerFinished += EndPlayerTurn;
        playerData = playerInstance.GetComponentInChildren<Player>();
        playerTyper.enabled = false;
        
        yield return new WaitForSeconds(1f);
        
    
        yield return new WaitForSeconds(0.5f);
        
        if (enemySpawner != null)
        {
            // yield return new WaitUntil(() => enemySpawner.GetAliveEnemyCount() > 0);

            enemies = enemySpawner.GetAliveEnemies();

            Debug.Log(enemies.Count);
            
            if (enemies.Count > 0)
            {
                currentTargetEnemy = enemies[0];
                
                foreach (Enemy enemy in enemies)
                {
                    enemy.OnEnemyDeath += HandleEnemyDeath;
                }
            }
            else
            {
                Debug.LogError("No enemies spawned!");
            }
        }
        
        yield return new WaitForSeconds(1f);
        
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void HandleEnemyDeath(Enemy deadEnemy)
    {
        enemies.Remove(deadEnemy);
        
        Debug.Log($"Enemy died! Remaining: {enemies.Count}");
        
        if (enemies.Count > 0)
        {
            currentTargetEnemy = enemies[0];
            currentEnemyIndex = 0;
        }
        else
        {
            currentTargetEnemy = null;
        }
        
        CheckIfDied();
    }

    private void PlayerTurn()
    {
        for (int i = 0; i < deckManager.maxHandSize; i++)
        {
            deckManager.DrawCard(handManager);
        }

        CheckIfDied();
        
        if (state != BattleState.WON && state != BattleState.LOST)
        {
            playerTyper.enabled = true;
            playerData.currentAP = playerData.player.attackPoin;
            turnText.text = "Player Turn";
        }
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

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.CastSpell();
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(1f);

        turnText.text = "";
        EndEnemyTurn();
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
        if (enemies.Count == 0 || enemies.TrueForAll(e => e == null || e.currentHP <= 0))
        {
            state = BattleState.WON;
            turnText.text = "YOU WIN!";
            return;
        }

        if (playerData.currentHP <= 0)
        {
            state = BattleState.LOST;
            turnText.text = "YOU LOST!";
            return;
        }
    }

    public Enemy GetCurrentTarget()
    {
        return currentTargetEnemy;
    }

    public List<Enemy> GetAliveEnemies()
    {
        return new List<Enemy>(enemies);
    }

    public void SwitchTarget()
    {
        if (enemies.Count <= 1) return;
        
        currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
        currentTargetEnemy = enemies[currentEnemyIndex];
        
        Debug.Log($"Target switched to: {currentTargetEnemy.gameObject.name}");
        
    }

    void OnDestroy()
    {
        if (playerTyper != null)
        {
            playerTyper.OnPlayerFinished -= EndPlayerTurn;
        }
        
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.OnEnemyDeath -= HandleEnemyDeath;
            }
        }
    }
}