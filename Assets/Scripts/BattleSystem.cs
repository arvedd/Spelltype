using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    
    [Header("Player")]
    public GameObject objectPlayer;
    public Transform playerBattleSpot;
    public SpellTyper playerTyper;
    public PlayerLevel playerLevel;
    private Player playerData;
    
    [Header("Enemy Spawner")]
    public EnemySpawner enemySpawner;
    public EnemyType enemyType;
    private List<Enemy> enemies = new List<Enemy>();
    private Enemy currentTargetEnemy;
    private int currentEnemyIndex = 0;
    
    [Header("UI")]
    public TextMeshProUGUI turnText;
    
    [Header("Managers")]
    public HandManager handManager;
    public DeckManager deckManager;
    public CounterInputManager counterInputManager;

    private List<Attack> activeEnemyAttacks = new List<Attack>();
    private int enemiesToAct = 0;
    private int enemiesFinishedCasting = 0;
    private bool allEnemiesHaveCast = false;
    private bool battleEnded = false;


    void Start()
    {
        handManager = FindAnyObjectByType<HandManager>();
        deckManager = FindAnyObjectByType<DeckManager>();
        counterInputManager = FindAnyObjectByType<CounterInputManager>();
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
        playerLevel = playerInstance.GetComponentInChildren<PlayerLevel>();
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
            counterInputManager.enabled = false;
            playerTyper.enabled = true;
            playerData.currentAP = playerData.player.attackPoin;
            Debug.Log($"{playerData.currentHP}");
            turnText.text = "Player Turn";
        }
    }

    private void EnemyTurn()
    {
        state = BattleState.ENEMYTURN; 
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        Debug.Log("It's Enemy Turn");
        turnText.text = "Enemy Turn";

        yield return new WaitForSeconds(0.5f);

        allEnemiesHaveCast = false;
        activeEnemyAttacks.Clear();

        // how many enemies will act this turn
        enemiesToAct = enemies.Count;
        enemiesFinishedCasting = 0;

        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                // run the enemy's behavior
                yield return enemy.DoTurn(this);

                // mark this enemy as finished acting
                enemiesFinishedCasting++;

                // short delay between enemy starts 
                if (enemiesFinishedCasting < enemiesToAct)
                {
                    float randomDelay = UnityEngine.Random.Range(0.5f, 0.7f);
                    yield return new WaitForSeconds(randomDelay);
                }
            }
            else
            {
                enemiesFinishedCasting++;
            }
        }

        // All enemy DoTurn coroutines have returned
        allEnemiesHaveCast = true;

        activeEnemyAttacks.RemoveAll(atk => atk == null);

        if (activeEnemyAttacks.Count == 0)
        {
            Debug.Log("No active enemy attacks after all enemies acted. Ending enemy turn.");
            EndEnemyTurn();
        }
        else
        {
            string activeList = "";
            foreach (var atk in activeEnemyAttacks)
            {
                if (atk != null)
                    activeList += $"{atk.gameObject.name} (caster: {atk.caster})\n";
                else
                    activeList += "[Null Attack Reference]\n";
            }

            Debug.Log($"Enemies have cast; waiting for {activeEnemyAttacks.Count} active attacks to resolve:\n{activeList}");
        }

        if (allEnemiesHaveCast && activeEnemyAttacks.Count == 0 && state == BattleState.ENEMYTURN)
        {
            Debug.Log("All enemies finished acting with no active attacks, ending enemy turn (safety fallback).");
            StartCoroutine(DelayedEndEnemyTurn());
        }
    }

    private void HandleEnemyAttackDestroyed(Attack destroyedAttack)
    {
        if (destroyedAttack == null) return;

        if (activeEnemyAttacks.Contains(destroyedAttack))
        {
            activeEnemyAttacks.Remove(destroyedAttack);
            Debug.Log($"Attack destroyed. Remaining active enemy attacks: {activeEnemyAttacks.Count}");
        }
        else
        {
            Debug.Log("Destroyed attack not found in active list. Re-checking.");
        }

        if (allEnemiesHaveCast && enemiesFinishedCasting >= enemiesToAct && activeEnemyAttacks.Count == 0 && state == BattleState.ENEMYTURN)
        {
            Debug.Log("All enemies have acted AND all attacks resolved, ending enemy turn.");
            StartCoroutine(DelayedEndEnemyTurn());
        }
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
            counterInputManager.enabled = true;
            turnText.text = "";
            EnemyTurn();
        }
    }

    public void CheckIfDied()
    {
       if (battleEnded) return;

        if (enemies.Count == 0 || enemies.TrueForAll(e => e == null || e.currentHP <= 0))
        {
            battleEnded = true;
            playerLevel.LevelUp();
            state = BattleState.WON;
            turnText.text = "YOU WIN!";
            playerData.WinAnim();

 
            int goldReward = 0;

            switch (enemyType)
            {
                case EnemyType.Normal:
                goldReward = UnityEngine.Random.Range(70, 91);
                break;

                case EnemyType.Elite:
                goldReward = UnityEngine.Random.Range(91, 121);
                break;

                case EnemyType.Boss:
                goldReward = 150;
                break;
            }

            GoldManager.AddGold(goldReward);

            Debug.Log("Gold Reward: " + goldReward);
            Debug.Log("Total Gold: " + GoldManager.GetGold());

            if (playerLevel.currentLevel > 5)
            {
                StartCoroutine(ChangeSceneAfterBattle("Ending"));
            }
             else
            {
                StartCoroutine(ChangeSceneAfterBattle("MapSelection"));
            
            }

            return;
        }

        if (playerData.currentHP <= 0)
        {
            battleEnded = true;
            state = BattleState.LOST;
            turnText.text = "YOU LOST!";
            StartCoroutine(ChangeSceneAfterBattle("MapSelection"));
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

    public void RegisterEnemyAttack(Attack attack)
    {
        if (attack == null) return;
        if (!activeEnemyAttacks.Contains(attack))
        {
            activeEnemyAttacks.Add(attack);
            Attack.OnAttackDestroyed -= HandleEnemyAttackDestroyed; 
            Attack.OnAttackDestroyed += HandleEnemyAttackDestroyed;
        }
    }

    private IEnumerator DelayedEndEnemyTurn()
    {
        yield return new WaitForSeconds(0.5f);
        EndEnemyTurn();
    }

    public IEnumerator ChangeSceneAfterBattle(string sceneName)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
        
    }

    public void CheckIfEnemyTurnShouldEnd()
    {
        if (allEnemiesHaveCast && activeEnemyAttacks.Count == 0 && state == BattleState.ENEMYTURN)
        {
            Debug.Log("Enemy turn ended via manual check.");
            StartCoroutine(DelayedEndEnemyTurn());
        }
    }

}